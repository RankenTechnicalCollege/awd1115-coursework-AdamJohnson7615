using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarLogger.Models
{
    public class Achievement
    {
        [Key]
        public int AchievementId { get; set; }

        [Required]
        public int GameId { get; set; } // FK to Game

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 1000)]
        public int Points { get; set; }

        // Navigation property
        [ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}
