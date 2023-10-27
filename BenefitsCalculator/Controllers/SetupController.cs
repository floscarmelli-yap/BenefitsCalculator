using AutoMapper;
using BenefitsCalculator.Data;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class SetupController : Controller
    {
        private readonly ILogger<SetupController> _logger;
        private readonly IBenefitsRepository _repository;
        private readonly IMapper _mapper;

        public SetupController(IBenefitsRepository repository, 
            ILogger<SetupController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<SetupDTO> setupList = new List<SetupDTO>();

            try
            {
                // get all setup data and send the list to the view
                setupList = _mapper.Map<List<SetupDTO>>
                    (await _repository.GetAllSetup());

                return View(setupList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup - Error while processing the request.");
            }

            // redirect to the error page if an exception occurred
            return RedirectToAction("Error", "App");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SetupDTO model)
        {
            // range properties validation
            if(model.MaxAgeLimit < model.MinAgeLimit)
            {
                ModelState.AddModelError(nameof(model.MaxAgeLimit),
                                         "Max Age Limit should be greater than Min Age Limit.");
            }
            if(model.MaxRange < model.MinRange)
            {
                ModelState.AddModelError(nameof(model.MaxRange),
                                         "Max Range should be greater than Min Range.");
            }

            try
            {
                // check the model state
                if (ModelState.IsValid)
                {
                    var newSetup = _mapper.Map<Setup>(model);

                    // add new setup data
                    _repository.AddEntity(newSetup);

                    if (await _repository.SaveAll())
                    {
                        // redirect to setup list view (index)
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup - Error while processing the request.");
            }

            // send an error message to the view
            ModelState.AddModelError("", "Failed to save.");

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // get setup data by id
                var setup = await _repository.GetSetupById(id);

                // if setup data is not null,
                // send setup data to the details view
                if (setup != null)
                {
                    return View(_mapper.Map<SetupDTO>(setup));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup - Error while processing the request.");
            }

            // redirect to the error page if setup data is not found
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }

        public async Task<IActionResult> Edit(int id) 
        {
            try
            {
                // get setup data by id
                var setup = await _repository.GetSetupById(id);

                // if setup data is not null,
                // send setup data to the edit view
                if (setup != null)
                {
                    return View(_mapper.Map<SetupDTO>(setup));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup - Error while processing the request.");
            }

            // redirect to the error page if setup data is not found
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SetupDTO model)
        {
            // range properties validation
            if (model.MaxAgeLimit < model.MinAgeLimit)
            {
                ModelState.AddModelError(nameof(model.MaxAgeLimit),
                                         "Max Age Limit should be greater than Min Age Limit.");
            }
            if (model.MaxRange < model.MinRange)
            {
                ModelState.AddModelError(nameof(model.MaxRange),
                                         "Max Range should be greater than Min Range.");
            }

            try
            {
                // check the model state
                if (ModelState.IsValid)
                {
                    var setup = _mapper.Map<Setup>(model);

                    // update entity if setup exists
                    if (await _repository.SetupExists(setup.Id))
                    {
                        _repository.UpdateEntity(setup);

                        if (await _repository.SaveAll())
                        {
                            // redirect to setup list view (index)
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        // send an error message to the view
                        ModelState.AddModelError("", "Setup data does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup - Error while processing the request.");
                ModelState.AddModelError("", "Failed to save.");
            }

            return View(model);
        }

        public IActionResult Delete(int id) 
        {
            // send setup id to the view
            var setup = new SetupDTO { Id = id };

            return View(setup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // get setup data by id
                var setup = await _repository.GetSetupById(id);

                if (setup != null)
                {
                    // delete setup and if successful,
                    // redirect to setup list view (index)
                    _repository.DeleteSetup(setup);

                    if (await _repository.SaveAll())
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup - Error while processing the request.");
            }

            // redirect to the error page if delete fails
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }
    }
}
