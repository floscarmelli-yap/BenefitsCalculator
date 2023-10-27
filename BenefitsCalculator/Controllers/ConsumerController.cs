using AutoMapper;
using BenefitsCalculator.Data;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class ConsumerController : Controller
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly IBenefitsRepository _repository;
        private readonly IMapper _mapper;

        public ConsumerController(IBenefitsRepository repository,
            ILogger<ConsumerController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool selection = false)
        {
            List<ConsumerDTO> consumerlist = new List<ConsumerDTO>();

            try
            {
                // get all consumer data
                consumerlist = _mapper.Map<List<ConsumerDTO>>
                    (await _repository.GetAllConsumers());

                // to identify if consumer list view is for selection view
                ViewBag.ForSelection = selection;

                return View(consumerlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer - Error while processing the request.");
            }

            // redirect to the error page if an exception occurred
            return RedirectToAction("Error", "App");
        }

        public async Task<IActionResult> Create()
        {
            var consumerWithSetupList = new ConsumerWithSetupListDTO();

            try
            {
                // assign setup list to the create consumer view
                // this is for the setup id select option list
                consumerWithSetupList.SetupList = _mapper.Map<List<SetupDTO>>(await _repository.GetAllSetup());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer - Error while processing the request.");
            }

            return View(consumerWithSetupList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConsumerDTO model)
        {
            try
            {
                // check the model state
                if (ModelState.IsValid)
                {
                    // convert consumer dto to consumer entity
                    var newConsumer = _mapper.Map<Consumer>(model);

                    // add consumer and save all changes
                    _repository.AddEntity(newConsumer);

                    if (await _repository.SaveAll())
                    {
                        // redirect to consumer list view (index)
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer - Error while processing the request.");
            }

            // send an error message to the view
            ModelState.AddModelError("", "Failed to save.");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // get consumer data by id
                var consumer = await _repository.GetConsumerById(id);

                // get setup list
                var setup = await _repository.GetAllSetup();

                // if consumer is not null, show the edit view
                if (consumer != null)
                {
                    return View(new ConsumerWithSetupListDTO
                    {
                        Id = consumer.Id,
                        SetupId = consumer.SetupId,
                        Name = consumer.Name,
                        BasicSalary = consumer.BasicSalary,
                        BirthDate = consumer.BirthDate,
                        SetupList = _mapper.Map<List<SetupDTO>>(setup)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer - Error while processing the request.");
            }

            // redirect to the error page if consumer data is null
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConsumerDTO model) 
        {
            try
            {
                // check the model state
                if (ModelState.IsValid)
                {
                    // convert consumer dto to consumer entity
                    var consumer = _mapper.Map<Consumer>(model);

                    // update entity if consumer exists
                    if (await _repository.ConsumerExists(consumer.Id))
                    {
                        _repository.UpdateEntity(consumer);

                        if (await _repository.SaveAll())
                        {
                            // redirect to consumer list view (index)
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        // send an error message to the view
                        ModelState.AddModelError("", "Consumer data does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer - Error while processing the request.");
            }

            // send an error message to the view
            ModelState.AddModelError("", "Failed to save.");

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            // send the consumer id to the view
            var consumer = new ConsumerDTO { Id = id };

            return View(consumer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // get consumer data for delete
                var consumer = await _repository.GetConsumerById(id);

                // check if consumer is not null
                if (consumer != null)
                {
                    // delete consumer and if successful,
                    // redirect to the consumer list (index) view
                    _repository.DeleteConsumer(consumer);

                    if (await _repository.SaveAll())
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer - Error while processing the request.");
            }

            // redirect to the error page if consumer delete fails
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }
    }
}
