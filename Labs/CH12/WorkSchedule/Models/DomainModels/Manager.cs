using System.ComponentModel.DataAnnotations;

namespace WorkSchedule.Models.DomainModels
{
    public class Manager
    {
            public Manager() => Positions = new HashSet<Position>();
            public int ManagerId { get; set; }            // Primary key

            [Display(Name = "First Name")]
            [StringLength(100, ErrorMessage = "First name may not exceed 100 characters.")]
            [Required(ErrorMessage = "Please enter a first name.")]

            public string FirstName { get; set; } = string.Empty;
            [Display(Name = "Last Name")]
            [StringLength(100, ErrorMessage = "Last name may not exceed 100 characters.")]
            [Required(ErrorMessage = "Please enter a last name.")] 
            
            public string LastName { get; set; } = string.Empty;
            public string FullName => $"{FirstName} {LastName}";
            public ICollection<Position> Positions { get; set; }
    }
}
