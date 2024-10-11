using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using WeatherForEver.Data;
using WeatherForEver.Models;
using WeatherForEver.Models.ViewModels;


namespace WeatherForEver.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ApplicationDbContext _dbContext;

        public AccountController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
            _dbContext = dbContext;
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
                if(_dbContext.Users.Any(x => x.UserName == user.UserName || x.Email == user.Email))
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (registerViewModel.Password != registerViewModel.RepeatPassword)
                {
                    ModelState.AddModelError("RepeatPassword", "Repeated password do not match");
                    return View(registerViewModel);
                }

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "User doesn't exist");
                    return View(loginViewModel);
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Wrong password.");
            }
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
