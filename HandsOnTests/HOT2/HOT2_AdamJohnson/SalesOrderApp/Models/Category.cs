using System.ComponentModel.DataAnnotations;

namespace SalesOrderApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; } = string.Empty;

        public Product? Product { get; set; }
    }
}
