using System.ComponentModel.DataAnnotations;

namespace Project1_FAQs.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; } = string.Empty; // URL-friendly
        public string Name { get; set; } = string.Empty;
    }
}
