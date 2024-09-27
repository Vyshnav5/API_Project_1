using System.ComponentModel.DataAnnotations;

namespace API_Project_1.Model
{
    public class Registration
    {
        [Key] 
        public int RId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ph { get; set; }
        [Required] 
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
