using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BenefitsCalculator.Data;
using BenefitsCalculator.Models;
using AutoMapper;
using BenefitsCalculator.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class BenefitsHistoryController : Controller
    {
        private readonly ILogger<BenefitsHistoryController> _logger;
        private readonly IBenefitsRepository _repository;
        private readonly IMapper _mapper;

        public BenefitsHistoryController(IBenefitsRepository repository,
            ILogger<BenefitsHistoryController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int id)
        {
            try
            {
                // get benefits history group data of consumer
                var result = await _repository.GetConsumerBenefitsHistGroups(id);

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

                // return data to the view
                return View(histGroupList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History - Error while processing the request.");
            }

            // redirect to the error page if an exception occurred
            return RedirectToAction("Error", "App");
        }

        public async Task<IActionResult> AllHistory() 
        {
            try
            {
                // get all benefits history group data
                var result = await _repository.GetAllBenefitsHistGroups();

                // assign data to a dto
                var histGroupList = result.Select(x => new HistGroupListDTO
                {
                    Id = x.Id,
                    BasicSalary = x.BasicSalary,
                    ConsumerName = x.Consumer.Name,
                    GuaranteedIssue = x.GuaranteedIssue,
                    CreatedDate = x.CreatedDate
                }).ToList();

                return View(histGroupList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History - Error while processing the request.");
            }

            // redirect to the error page if an exception occurred
            return RedirectToAction("Error", "App");
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // get benefits history and its associated
                // groupings and consumer data
                var result = await _repository.GetBenefitsHistGroupById(id);

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

                return View(historyGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History - Error while processing the request.");
            }

            // redirect to the error page if an exception occurred
            return RedirectToAction("Error", "App");
        }

        public IActionResult Delete(int id)
        {
            // send benefits history group id to the view
            var histGroup = new HistGroupDTO { Id = id };

            return View(histGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // get benefits history and its associated groupings for deletion
                var histGroup = await _repository.GetBenefitsHistGroupForDelete(id);

                if (histGroup != null)
                {
                    // delete histories and its groupings and
                    // redirect to the benefits history list page
                    _repository.DeleteHistory(histGroup);

                    // if save is successful, redirect to the list (index) of consumer benefits history 
                    if (await _repository.SaveAll())
                    {
                        return RedirectToAction("Index", new { id = histGroup.ConsumerId});
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Benefits History - Error while processing the request.");
            }

            // redirect to the error page if delete fails
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }
    }
}
