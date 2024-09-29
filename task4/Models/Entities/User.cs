using System.ComponentModel.DataAnnotations.Schema;

namespace task4.Models.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }

        [Column(TypeName = "datetime")] // Явно указываем тип данных
        public DateTime RegistrationTime { get; set; }

        [Column(TypeName = "datetime")] // Явно указываем тип данных
        public DateTime LastLoginTime { get; set; }

        public string Password { get; set; }

    }
}
