using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WorkSchedule.Models.DomainModels
{
    public class Position
    {
        public int PositionId { get; set; } // Primary key
        [StringLength(200, ErrorMessage = "Title may not exceed 200 characters.")]
        [Required(ErrorMessage = "Please enter a position title.")]
        public string Title { get; set; } = string.Empty;
        [Range(100, 500, ErrorMessage = "Position number must be between 100 and 500.")]
        [Required(ErrorMessage = "Please enter a position number.")]
        public int? Number { get; set; }
        [Display(Name = "Time")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter numbers only for position time.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Position time must be 4 characters long.")]
        [Required(ErrorMessage = "Please enter a position time (in military time format).")]
        public string MilitaryTime { get; set; } = string.Empty;        
        public int ManagerId { get; set; }
        // Foreign key property
        [ValidateNever]
        public Manager Manager { get; set; } = null!;
        // Navigation property
        public int DayId { get; set; }
        // Foreign key property
        [ValidateNever]
        public Day Day { get; set; } = null!;
        // Navigation property
    }
}
