using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; 

namespace PlushShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [Required, StringLength(60)]
        public string Series { get; set; } = "";

        [Required, StringLength(60)]
        public string Company { get; set; } = "";

        [Required, StringLength(60)]
        public string MediaFormat { get; set; } = "";

        [Required, StringLength(150)]
        public string Slug { get; set; } = "";

        [StringLength(200)]
        public string ImageFileName { get; set; } = "placeholder.png";

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // For image uploads in admin
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
