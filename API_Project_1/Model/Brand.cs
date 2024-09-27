using System.ComponentModel.DataAnnotations;

namespace API_Project_1.Model
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        public string IsActive { get; set; }
    }
}
