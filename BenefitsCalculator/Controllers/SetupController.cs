using AutoMapper;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class SetupController : Controller
    {
        private readonly ILogger<SetupController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public SetupController(ILogger<SetupController> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // request to get all setups
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "https://localhost:7174/api/setups");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if(response.IsSuccessStatusCode)
                {
                    // return setup list to the view
                    return View(await response.Content.ReadFromJsonAsync<IList<SetupDTO>>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Controller - Error while processing the request.");
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
                    // request to create new setup
                    var request = new HttpRequestMessage(HttpMethod.Post,
                        "https://localhost:7174/api/setups");

                    var client = _clientFactory.CreateClient();
                    var response = await client.PostAsJsonAsync<SetupDTO>(request.RequestUri, model);

                    if (response.IsSuccessStatusCode)
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
                // request to get setup data by id
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://localhost:7174/api/setups/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                // if request status is successful,
                // send setup data to the details view
                if (response.IsSuccessStatusCode)
                {
                    return View(await response.Content.ReadFromJsonAsync<SetupDTO>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Controller - Error while processing the request.");
            }

            // redirect to the error page if setup data is not found
            // or if an exception occurred
            return RedirectToAction("Error", "App");
        }

        public async Task<IActionResult> Edit(int id) 
        {
            try
            {
                // request to get setup data by id
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://localhost:7174/api/setups/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                // if request status is successful,
                // send setup data to the edit view
                if (response.IsSuccessStatusCode)
                {
                    return View(await response.Content.ReadFromJsonAsync<SetupDTO>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setup Controller - Error while processing the request.");
            }

            // redirect to the error page if request is unsuccessful
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
                    // request to edit setup data
                    var request = new HttpRequestMessage(HttpMethod.Put,
                        "https://localhost:7174/api/setups");

                    var client = _clientFactory.CreateClient();
                    var response = await client.PutAsJsonAsync<SetupDTO>(request.RequestUri, model);

                    if (response.IsSuccessStatusCode)
                    {
                        // redirect to setup list view (index)
                        return RedirectToAction("Index");
                    }
                    else if(response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // send an error message to the view if setup is not found
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
                // request to delete setup data
                var request = new HttpRequestMessage(HttpMethod.Delete,
                    $"https://localhost:7174/api/setups/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync(request.RequestUri);

                if (response.IsSuccessStatusCode)
                {
                    // on a successful delete,
                    // redirect to setup list view (index)
                    return RedirectToAction(nameof(Index));
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
