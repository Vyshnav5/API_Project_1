using System.ComponentModel.DataAnnotations;

namespace API_Project_1.Model
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password{get; set; }
    }
}
