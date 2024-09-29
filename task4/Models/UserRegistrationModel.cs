using System.ComponentModel.DataAnnotations;

namespace task4.Models
{
	public class UserRegistrationModel
	{
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]

        public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public DateTime RegistrationTime { get; set; }

		public DateTime LastLoginTime { get; set; }

		public string Status { get; set; } = "active";

		public UserRegistrationModel()
		{
			RegistrationTime = DateTime.Now; // Set to current date
			LastLoginTime = DateTime.Now;
		}

	}
}
