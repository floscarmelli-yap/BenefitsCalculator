using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BenefitsCalculator.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ILogger<AccountController> logger, 
            SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            // check if model state is valid
            if (ModelState.IsValid)
            {
                // sign in using the inputted credentials
                var result = await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    false,
                    false);

                // if succeeded, redirect to the home page
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in");
                    return RedirectToAction("Index", "App");
                }
            }

            _logger.LogError("User failed to login");

            // return an error to the view on failed login
            ModelState.AddModelError("", "Failed to login");

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User logged out");

            // logout of app and redirect to login page
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
