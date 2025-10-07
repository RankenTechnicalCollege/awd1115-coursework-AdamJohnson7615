using System.ComponentModel.DataAnnotations;

namespace Project1_TripsLog.ViewModels
{
    public class TripAccommodations
    {
        [Display(Name = "Phone")]
        public string? AccommodationPhone { get; set; }

        [Display(Name = "Email")]
        public string? AccommodationEmail { get; set; }
    }
}
