using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using task4.Data;
using task4.Models.Entities;

namespace task4.Controllers
{
    public class ListController : Controller
    {
        private readonly AppDbContext dbContext;

        public ListController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
		private void ShowError(string[] selectedUsers)
		{
			if (selectedUsers == null || !selectedUsers.Any())
			{
				ViewData["MessageValidation"] = "No users selected. Please, select any user";
			}
		}
		private string GetCurrentUserEmail()
		{
			return User.FindFirst(ClaimTypes.Email)?.Value;
		}

		private async Task ChangeStatusAndSaveChanges(List<User> usersToBlock, string status)
		{
			foreach (var user in usersToBlock)
			{
				user.Status =status;
			}
			await dbContext.SaveChangesAsync();
		}

		private async Task DeleteUsersAndSaveChanges(List<User> usersToDelete)
		{
			if (usersToDelete.Any())
			{
				dbContext.Users.RemoveRange(usersToDelete);
				await dbContext.SaveChangesAsync();
			}
		}
		private async Task<List<User>> GetUsersToProcess(string[] selectedUsers)
		{
			return await dbContext.Users
								  .Where(user => selectedUsers.Contains(user.Email))
								  .ToListAsync();
		}


		private bool IsCurrentUserSelected(string[] selectedUsers, string currentUserEmail)
		{
			return selectedUsers.Contains(currentUserEmail);
		}
		private async Task<IActionResult> RedirectCurrentUser()
		{
			await dbContext.SaveChangesAsync();
			return RedirectToAction("Login", "Access");
		}



		[HttpGet]
        public async Task<IActionResult> ShowList()
        {
            var Users = await dbContext.Users.ToListAsync();
            return View(Users);
        }

		[HttpPost]
		public async Task<IActionResult> BlockUsers(string[] selectedUsers)
		{
			ShowError(selectedUsers);

			var usersToBlock = await GetUsersToProcess(selectedUsers);
			var currentUserEmail = GetCurrentUserEmail();
			await ChangeStatusAndSaveChanges(usersToBlock,"blocked");

			if (IsCurrentUserSelected(selectedUsers, currentUserEmail))
			{
				return await RedirectCurrentUser();
			}
			return RedirectToAction("ShowList", "List");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteUsers(string[] selectedUsers)
		{
			ShowError(selectedUsers);
			var usersToDelete = await GetUsersToProcess(selectedUsers);
			var currentUserEmail = GetCurrentUserEmail();
			await DeleteUsersAndSaveChanges(usersToDelete);
			if (IsCurrentUserSelected(selectedUsers, currentUserEmail))
			{
				return await RedirectCurrentUser();
			}

			return RedirectToAction("ShowList", "List");
		}

		[HttpPost]
		public async Task<IActionResult> UnblockUsers(string[] selectedUsers)
		{
			ShowError(selectedUsers);
			var usersToUnblock = await GetUsersToProcess(selectedUsers);
			await ChangeStatusAndSaveChanges(usersToUnblock, "active");
			return RedirectToAction("ShowList", "List"); 
		}

	}
}
