using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using WeatherForEver.Models;
using WeatherForEver.Models.ViewModels;


namespace WeatherForEver.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var user = new ApplicationUser()
            {
                UserName = registerViewModel.Name,
                Email = registerViewModel.Email,
            };

            if (ModelState.IsValid)
            {
                var folder = $"photos/{registerViewModel.Name}_{registerViewModel.AccountPhoto.FileName}";

                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                user.AccountPhoto = folder;

                await registerViewModel.AccountPhoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}
