using AutoMapper;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;
using System.Net.Http.Json;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class ConsumerController : Controller
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ConsumerController(ILogger<ConsumerController> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index(bool selection = false)
        {
            try
            {
                // to identify if consumer list view is for selection view
                ViewBag.ForSelection = selection;

                // request to get all consumers
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "https://localhost:7174/api/consumers");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // return consumer list to the view
                    return View(await response.Content.ReadFromJsonAsync<IEnumerable<ConsumerDTO>>());
                }
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
            // return to view with a list of setup ids for selection
            return View(new ConsumerWithSetupIdsDTO
            {
                SetupIds = await GetSetupIds()
            }) ;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConsumerWithSetupIdsDTO model)
        {
            try
            {
                // check the model state
                if (ModelState.IsValid)
                {
                    // convert to consumer dto
                    var newConsumer = new ConsumerDTO
                    {
                        Name = model.Name,
                        SetupId = model.SetupId,
                        BasicSalary = model.BasicSalary,
                        BirthDate = model.BirthDate,
                    };

                    // request to create new consumer
                    var request = new HttpRequestMessage(HttpMethod.Post,
                        "https://localhost:7174/api/consumers");

                    var client = _clientFactory.CreateClient();
                    var response = await client.PostAsJsonAsync<ConsumerDTO>(request.RequestUri, newConsumer);

                    if (response.IsSuccessStatusCode)
                    {
                        // redirect to consumer list view (index)
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    // return setup id array to the view for 
                    // the selection option list
                    model.SetupIds = await GetSetupIds();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Create - Error while processing the request.");
            }

            // send an error message to the view
            ModelState.AddModelError("", "Failed to save.");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // request to get consumer data by id
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://localhost:7174/api/consumers/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                // if response status is success,
                // show the edit view
                if (response.IsSuccessStatusCode)
                {
                    var consumer = await response.Content.ReadFromJsonAsync<ConsumerDTO>();
                    consumer ??= new ConsumerDTO();

                    return View(new ConsumerWithSetupIdsDTO
                    {
                        Id = consumer.Id,
                        SetupId = consumer.SetupId,
                        Name = consumer.Name,
                        BasicSalary = consumer.BasicSalary,
                        BirthDate = consumer.BirthDate,
                        SetupIds = await GetSetupIds(),
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Edit - Error while processing the request.");
            }

            // redirect to the error page if reponse status is not success
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConsumerWithSetupIdsDTO model) 
        {
            try
            {
                // check the model state
                if (ModelState.IsValid)
                {
                    // convert to consumer dto
                    var consumer = new ConsumerDTO
                    {
                        Id = model.Id,
                        Name = model.Name,
                        SetupId = model.SetupId,
                        BasicSalary = model.BasicSalary,
                        BirthDate = model.BirthDate,
                    };

                    // request to edit consumer data
                    var request = new HttpRequestMessage(HttpMethod.Put,
                        "https://localhost:7174/api/consumers");

                    var client = _clientFactory.CreateClient();
                    var response = await client.PutAsJsonAsync<ConsumerDTO>(request.RequestUri, consumer);

                    if (response.IsSuccessStatusCode)
                    {
                        // redirect to consumer list view (index)
                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // send an error message to the view if consumer is not found
                        ModelState.AddModelError("", "Consumer data does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Edit - Error while processing the request.");
            }

            // return setup id array to the view for 
            // the selection option list
            model.SetupIds = await GetSetupIds();

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
                // request to get consumer data for delete
                var request = new HttpRequestMessage(HttpMethod.Delete,
                    $"https://localhost:7174/api/consumers/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync(request.RequestUri);

                if (response.IsSuccessStatusCode)
                {
                    // redirect to the consumer list (index) view
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer Delete - Error while processing the request.");
            }

            // redirect to the error page if consumer delete fails
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }

        private async Task<int[]> GetSetupIds()
        {
            var setupIds = new int[0];

            try
            {
                // request to get all setups
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "https://localhost:7174/api/setups");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var setupList = await response.Content.
                        ReadFromJsonAsync<IList<SetupDTO>>();

                    if (setupList != null)
                    {
                        // assign setup id array to the create consumer view
                        // this is for the setup id select option list
                        setupIds = setupList.Select(x => x.Id).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer GetSetupIds - Error while processing the request.");
            }
            
            return setupIds;
        }
    }
}
