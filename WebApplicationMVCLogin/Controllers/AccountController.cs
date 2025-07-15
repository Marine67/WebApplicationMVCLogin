using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplicationMVCLogin.Models;
using WebApplicationMVCLogin.Views.DBContect;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplicationMVCLogin.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _ctx;

        public AccountController(ApplicationDBContext db)
        {
            _ctx = db;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string command)
        {
            Debug.WriteLine("Just Check");
            //Console.WriteLine("con check");
            Trace.WriteLine("Tracing Index action entered");
            if (!ModelState.IsValid)
                return View(model);

            if (command == "login")
            {
                string pwd=HashPassword(model.Password);
                var user = _ctx.loginViewModels
                .FirstOrDefault(u => u.Email == model.Email && u.Password == pwd); // Use hashing in prod

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                var claims = new List<Claim>
        {

            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }

            else if (command == "register")
            {
                model.Password = HashPassword(model.Password);
                _ctx.loginViewModels.Add(model);
                _ctx.SaveChanges();

                TempData["Alert.Type"] = "success";       
                TempData["Alert.Message"] = "You registered successfully!";

                return RedirectToAction("Login");
            }
            return View(model);
        }

        public string HashPassword(string pwd)
        {
            using(var sha=SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(pwd);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
