using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarLogger.Models
{
    public class Achievement
    {
        [Key]
        public int AchievementId { get; set; }

        [Required]
        [Display(Name = "Game")]
        public int GameId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 1000)]
        public int Points { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}