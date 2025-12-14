using System.ComponentModel.DataAnnotations;

namespace StarLogger.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? CoverImageUrl { get; set; }
        public string? Platform { get; set; }
        public string? IgdbId { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}