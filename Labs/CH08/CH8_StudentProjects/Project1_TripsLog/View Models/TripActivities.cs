using System.ComponentModel.DataAnnotations;

namespace Project1_TripsLog.ViewModels
{
    public class TripActivities
    {
        [Display(Name = "Activity 1")]
        public string? Activity1 { get; set; }

        [Display(Name = "Activity 2")]
        public string? Activity2 { get; set; }

        [Display(Name = "Activity 3")]
        public string? Activity3 { get; set; }
    }
}