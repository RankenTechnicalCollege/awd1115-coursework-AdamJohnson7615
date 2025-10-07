using System;
using System.ComponentModel.DataAnnotations;

namespace Project1_TripsLog.ViewModels
{
    public class TripDestination
    {
        [Required(ErrorMessage = "Please enter a destination.")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the accommodation name.")]
        [Display(Name = "Accommodation")]
        public string? AccommodationName { get; set; }

        [Required(ErrorMessage = "Please select a start date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please select an end date.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
