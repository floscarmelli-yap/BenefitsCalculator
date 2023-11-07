using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BenefitsCalculator.Models;
using AutoMapper;
using BenefitsCalculator.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BenefitsCalculator.Data.Repositories;

namespace BenefitsCalculator.Controllers
{
    [Authorize]
    public class BenefitsHistoryController : Controller
    {
        private readonly ILogger<BenefitsHistoryController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public BenefitsHistoryController(ILogger<BenefitsHistoryController> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index(int id)
        {
            try
            {
                // request to get benefits history group data of consumer
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://localhost:7174/api/benefits/consumer_histories/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // return data to the view
                    return View(await response.Content.ReadFromJsonAsync<IEnumerable<HistGroupListDTO>>());
                }
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
                // request to get all benefits history group data
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "https://localhost:7174/api/benefits/histories");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // return data to the view
                    return View(await response.Content.ReadFromJsonAsync<IEnumerable<HistGroupListDTO>>());
                }
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
                // request to get benefits history and its associated
                // groupings and consumer data
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://localhost:7174/api/benefits/histories/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // return data to the view
                    return View(await response.Content.ReadFromJsonAsync<HistGroupDetailsDTO>());
                }
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
                // request to get benefits history and its associated groupings for deletion
                var request = new HttpRequestMessage(HttpMethod.Delete,
                    $"https://localhost:7174/api/benefits/histories/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync(request.RequestUri);

                if (response.IsSuccessStatusCode)
                {
                    var consumerId = await response.Content.ReadAsStringAsync();

                    // redirect to the list (index) of consumer benefits history
                    return RedirectToAction("Index", new { id = int.Parse(consumerId) });
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
