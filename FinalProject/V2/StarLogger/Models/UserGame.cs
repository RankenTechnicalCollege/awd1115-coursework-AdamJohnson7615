using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarLogger.Models
{
    public class UserGame
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game? Game { get; set; }

        [Range(1, 10)]
        public int? Rating { get; set; }

        public CompletionStatus Status { get; set; } = CompletionStatus.NotStarted;

        public string? AchievementsEarned { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}