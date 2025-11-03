using System.ComponentModel.DataAnnotations;

namespace DocStop.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username must be at most 100 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?\d{10,15}$",
            ErrorMessage = "Phone number must be 10-15 digits, optional leading +. Example: +12345678901")]
        public string Phone { get; set; }
    }
}
