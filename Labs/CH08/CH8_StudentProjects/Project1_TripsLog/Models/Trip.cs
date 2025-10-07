namespace Project1_TripsLog.Models
{
    public class Trip
    {
        public int Id { get; set; }

        // Destination info
        public string Destination { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Accommodation info
        public string? AccommodationName { get; set; }
        public string? AccommodationPhone { get; set; }
        public string? AccommodationEmail { get; set; }

        // Activities info
        public string? Activities { get; set; }
    }
}
