using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequestModel model)
        {
            // Cookie based Authentication
            // Forms Authentication
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userService.Login(model);
            if (user==null)
            {
                return View();
            }
            
            // store some information in the Cookies, Authentication cookie... Claims
           var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                
            };
            
            // Identity class... and Principle
            // go to an bar => check your identity=> Driving Licence
            // go to Airport => check your passport
            // Create a Movie => calim with role value as Admin

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            // create the cookies
            // HttpContext

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect("~/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userService.Register(model);
           return RedirectToAction("Login");
        }
    }
}