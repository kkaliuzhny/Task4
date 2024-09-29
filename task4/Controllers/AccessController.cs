using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using task4.Data;
using task4.Models;

namespace task4.Controllers
{
    public class AccessController : Controller
    {
        private readonly AppDbContext _context;

        public AccessController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel userLoginModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginModel.Email);
            if (user is null)
            {
                ViewData["ValidateMessage"] = "Invalid login attempt. User not found.";
                return View();
            }
            bool isPasswordValid = PasswordHasher.VerifyPassword(userLoginModel.Password, user.Password);

            if (!isPasswordValid)
            {
                ViewData["ValidateMessage"] = "Invalid login attempt. Incorrect password.";
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                 new Claim("Status", user.Status)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (user.Status == "blocked")
            {
                ViewData["ValidateMessage"] = "You are blocked";
                return View();
            }
            user.LastLoginTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowList", "List");
        }

        [HttpPost]
		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(); 
			return RedirectToAction("Login", "Access"); 
		}

	}
}
