using System.ComponentModel.DataAnnotations;

namespace Project1_ContactManager.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}