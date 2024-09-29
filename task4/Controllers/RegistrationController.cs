using Microsoft.AspNetCore.Mvc;
using System;
using task4.Data;
using task4.Models;
using task4.Models.Entities;

namespace task4.Controllers
{
	public class RegistrationController : Controller
	{
		private readonly AppDbContext dbContext;

		public RegistrationController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(UserRegistrationModel userModel)
		{
            if (userModel.Name == userModel.Email)
            {
                ModelState.AddModelError("", "The name and the email must be different");
            }
			if (ModelState.IsValid)
			{
                var user = new User
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Password = PasswordHasher.HashPassword(userModel.Password),
                    RegistrationTime = userModel.RegistrationTime,
                    LastLoginTime = userModel.LastLoginTime,
                    Status = userModel.Status
                };

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Login", "Access");
            }
			return View(userModel);
		}
	}
}
