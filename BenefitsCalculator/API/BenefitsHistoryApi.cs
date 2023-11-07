using AutoMapper;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace BenefitsCalculator.API
{
    [Route("api/benefits")]
    [ApiController]
    public class BenefitsHistoryApi : ControllerBase
    {
        private readonly IBenefitsHistoryRepository _benefitsHistRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger<BenefitsHistoryApi> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public BenefitsHistoryApi(IBenefitsHistoryRepository benefitsHistRepository, 
            ICommonRepository commonRepository,
            ILogger<BenefitsHistoryApi> logger, 
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _benefitsHistRepository = benefitsHistRepository;
            _commonRepository = commonRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        // api/benefits/histories
        [HttpGet("histories")]
        public async Task<ActionResult<IEnumerable<HistGroupListDTO>>> Get()
        {
            try
            {
                // get all benefits history group data
                var result = await _benefitsHistRepository.GetAllBenefitsHistGroups();

                // assign data to a dto
                var histGroupList = result.Select(x => new HistGroupListDTO
                {
                    Id = x.Id,
                    BasicSalary = x.BasicSalary,
                    ConsumerName = x.Consumer.Name,
                    GuaranteedIssue = x.GuaranteedIssue,
                    CreatedDate = x.CreatedDate
                }).ToList();

                return Ok(histGroupList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History Get() - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/benefits/consumer_histories/consumerId
        [HttpGet("consumer_histories/{consumerId}")]
        public async Task<ActionResult<IEnumerable<HistGroupListDTO>>> GetConsumerHistories(int consumerId)
        {
            try
            {
                // get benefits history group data of consumer
                var result = await _benefitsHistRepository.GetConsumerBenefitsHistGroups(consumerId);

                // assign result to a dto
                var histGroupList = result.Select(x => new HistGroupListDTO
                {
                    Id = x.Id,
                    BasicSalary = x.BasicSalary,
                    ConsumerId = x.ConsumerId,
                    ConsumerName = x.Consumer.Name,
                    GuaranteedIssue = x.GuaranteedIssue,
                    CreatedDate = x.CreatedDate
                }).ToList();

                return Ok(histGroupList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History GetConsumerHistories(consumerId) - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/benefits/histories/id
        [HttpGet("histories/{id}")]
        public async Task<ActionResult<HistGroupDetailsDTO>> GetHistoryDetails(int id)
        {
            try
            {
                // get benefits history and its associated
                // groupings and consumer data
                var result = await _benefitsHistRepository.GetBenefitsHistGroupById(id);

                // assign data to a dto
                var historyGroup = new HistGroupDetailsDTO
                {
                    Id = result.Id,
                    ConsumerId = result.ConsumerId,
                    BasicSalary = result.BasicSalary,
                    ConsumerName = result.Consumer.Name,
                    GuaranteedIssue = result.GuaranteedIssue,
                    BenefitsHistories = _mapper.Map<List<BenefitsHistDTO>>(result.BenefitsHistories)
                };

                return Ok(historyGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History GetHistoryDetails(id) - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/benefits
        [HttpPost]
        public async Task<ActionResult<int>> Post(HistGroupDTO histGroup)
        {
            try
            {
                // convert HistGroupDTO to BenefitsHistGroup
                var newHistGroup = _mapper.Map<BenefitsHistGroup>(histGroup);

                // assign created by value if user is not null
                // note: saving will not fail even if createdby value is empty
                if (histGroup.CreatedBy != null)
                {
                    newHistGroup.CreatedBy = await _userManager.FindByNameAsync(histGroup.CreatedBy);
                }

                // save the computed benefits
                _commonRepository.AddEntity(newHistGroup);

                if (await _commonRepository.SaveAll())
                {
                    return Ok(newHistGroup.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History Post(HistGroupDTO histGroup) - Error while processing the request.");
            }

            return BadRequest();
        }

        /// api/benefits/histories/id
        [HttpDelete("histories/{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                // get benefits history and its associated groupings for deletion
                var histGroup = await _benefitsHistRepository.GetBenefitsHistGroupForDelete(id);

                if (histGroup != null)
                {
                    // delete histories and its groupings
                    _benefitsHistRepository.DeleteHistory(histGroup);

                    if (await _commonRepository.SaveAll())
                    {
                        return Ok(histGroup.ConsumerId);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History Delete(id) - Error while processing the request.");
            }

            return BadRequest();
        }
    }
}
