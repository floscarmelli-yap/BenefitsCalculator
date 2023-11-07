using AutoMapper;
using BenefitsCalculator.ComputationLogic;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class ComputeBenefitsController : Controller
    {
        private readonly ILogger<ComputeBenefitsController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ComputeBenefitsController(ILogger<ComputeBenefitsController> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
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
                // request to get consumer and setup data
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://localhost:7174/api/consumers/{id}/consumer-setup");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                // if request status is success,
                // send consumer and setup data to the view
                if (response.IsSuccessStatusCode)
                {
                    return View(await response.Content.ReadFromJsonAsync<ConsumerSetupDTO>());
                }
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
                var result = benefits.ComputeBenefits();

                // check if benefits list is not null and empty
                if (result.BenefitsList != null && result.BenefitsList.Count > 0)
                {
                    // assign value to created by
                    if(User.Identity != null)
                    {
                        result.CreatedBy = User.Identity.Name;
                    }

                    // request to create new benefits data
                    var request = new HttpRequestMessage(HttpMethod.Post, 
                        "https://localhost:7174/api/benefits");

                    var client = _clientFactory.CreateClient();
                    var response = await client.PostAsJsonAsync(request.RequestUri, result);

                    if (response.IsSuccessStatusCode)
                    {
                        var histGroupId = await response.Content.ReadAsStringAsync();

                        // redirect to the details page
                        return RedirectToAction("Details", "BenefitsHistory", new { id = int.Parse(histGroupId) });
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
