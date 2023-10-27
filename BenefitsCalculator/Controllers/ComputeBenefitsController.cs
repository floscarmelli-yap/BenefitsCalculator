using AutoMapper;
using BenefitsCalculator.ComputationLogic;
using BenefitsCalculator.Data;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class ComputeBenefitsController : Controller
    {
        private readonly ILogger<ComputeBenefitsController> _logger;
        private readonly IBenefitsRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ComputeBenefitsController(IBenefitsRepository repository,
            ILogger<ComputeBenefitsController> logger,
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ConsumerSelection() 
        {
            // redirect to the consumer selection page
            return RedirectToAction("Index", "Consumer", new { selection = true});
        }

        [HttpGet]
        public async Task<IActionResult> ComputationDetails(int id)
        {
            try
            {
                // get consumer and setup data
                var result = await _repository.GetConsumersIncludingSetupById(id);

                // assign result to a dto
                var consumerSetupModel = new ConsumerSetupDTO
                {
                    Setup = _mapper.Map<SetupDTO>(result.Setup),
                    Consumer = _mapper.Map<ConsumerDTO>(result)
                };

                return View(consumerSetupModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Compute Benefits - Error while processing the request.");
            }

            // redirect to the error page if an exception occurred
            return RedirectToAction("Error", "App");
        }

        [HttpPost]
        public async Task<IActionResult> ComputationDetails(ConsumerSetupDTO model)
        {
            // pass consumer and setup details for computation
            BenefitsComputation benefits = new BenefitsComputation(model);

            try
            {
                // compute consumer benefits
                var resultList = benefits.ComputeBenefits();

                // check if benefits list is not null and empty
                if (resultList.BenefitsList != null && resultList.BenefitsList.Count > 0)
                {
                    BenefitsHistGroup histGroup = new BenefitsHistGroup
                    {
                        ConsumerId = resultList.ConsumerId,
                        CreatedDate = resultList.CreatedDate,
                        GuaranteedIssue = resultList.GuaranteedIssue,
                        BasicSalary = resultList.BasicSalary,
                        BenefitsHistories = _mapper.Map<List<BenefitsHistory>>(resultList.BenefitsList)
                    };

                    // assign created by value if user is not null
                    // note: saving will not fail even if createdby value is empty
                    if (User.Identity != null && User.Identity.Name != null)
                    {
                        histGroup.CreatedBy = await _userManager.FindByNameAsync(User.Identity.Name);
                    }

                    // save the computed benefits and redirect to the details page
                    _repository.AddEntity(histGroup);

                    if (await _repository.SaveAll())
                    {
                        return RedirectToAction("Details", "BenefitsHistory", new { id = histGroup.Id });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Compute Benefits - Error while processing the request.");
            }

            // redirect to the error page if the adding of computed benefits
            // data has failed or if an exception occurred
            return RedirectToAction("Error", "App");
        }
    }
}
