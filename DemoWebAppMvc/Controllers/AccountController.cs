using DemoWebAppMvc.Data;
using DemoWebAppMvc.Models;
using DemoWebAppMvc.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebAppMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }

        public IActionResult Login()
        {
            var respons = new LoginViewModel();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                //user Found, checking password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                //password Correct
                if(passwordCheck) 
                {

                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                //Password Incorrect 
                TempData["Error"] = "Wrong credentials. Please try again.";
                return View(loginViewModel);
            }
            TempData["Error"] = "Wrong Credentials. please try again";
            return View(loginViewModel);

        }

        public IActionResult Register()
        {
            var Response = new RegisterViewModel();
            return View(Response);
        }






        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid)
            { 
                return View(registerViewModel);
            }
            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This Email Address is Already in use";
                return View(registerViewModel);

            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,

            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if(newUserResponse.Succeeded)
            {
               
                //await _userManager.AddToRoleAsync(newUser, "admin");
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Race");
        }
    }
}
