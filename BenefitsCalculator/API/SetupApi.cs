using AutoMapper;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace BenefitsCalculator.API
{
    [Route("api/setups")]
    [ApiController]
    public class SetupApi : ControllerBase
    {
        private readonly ISetupRepository _setupRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger<SetupApi> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Setup Service Constructor
        /// </summary>
        /// <param name="setupRepository"></param>
        /// <param name="mapper"></param>
        public SetupApi(ISetupRepository setupRepository, ICommonRepository commonRepository,
            ILogger<SetupApi> logger, IMapper mapper)
        {
            _setupRepository = setupRepository;
            _commonRepository = commonRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // api/setups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetupDTO>>> Get()
        {
            try
            {
                // get all setup data
                var setups = await _setupRepository.GetAllSetup();

                return Ok(_mapper.Map<List<SetupDTO>>(setups));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Get() - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/setups/id
        [HttpGet("{id}")]
        public async Task<ActionResult<SetupDTO>> Get(int id)
        {
            try
            {
                // get setup data by id
                var setup = await _setupRepository.GetSetupById(id);

                // if setup data is not null,
                // return ok, else return not found
                if (setup != null)
                {
                    return Ok(_mapper.Map<SetupDTO>(setup));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Get(int id) - Error while processing the request.");
            }

            return BadRequest();
        }

        // api/setups
        [HttpPost]
        public async Task<ActionResult> Post(SetupDTO setup)
        {
            try
            {
                // convert SetupDTO to Setup
                var newSetup = _mapper.Map<Setup>(setup);

                // add new setup data
                _commonRepository.AddEntity(newSetup);

                if (await _commonRepository.SaveAll())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Post(SetupDTO setup) - Error while processing the request.");
            }

            return BadRequest();

        }

        // api/setups
        [HttpPut]
        public async Task<ActionResult> Put(SetupDTO setup)
        {
            try
            {
                // convert SetupDTO to Setup
                var modSetup = _mapper.Map<Setup>(setup);

                // update entity if setup exists
                if (await _setupRepository.SetupExists(modSetup.Id))
                {
                    _commonRepository.UpdateEntity(modSetup);

                    if (await _commonRepository.SaveAll())
                    {
                        return Ok();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Put(SetupDTO setup) - Error while processing the request.");
            }

            return BadRequest();

        }

        // api/setups/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // get setup data by id
                var setup = await _setupRepository.GetSetupById(id);

                if (setup != null)
                {
                    // delete setup
                    _setupRepository.DeleteSetup(setup);

                    if (await _commonRepository.SaveAll())
                    {
                        return Ok();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Delete(int id) - Error while processing the request.");
            }

            return BadRequest();
        }
    }
}
