using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarLogger.Models
{
    public class Game
    {
        public int GameId { get; set; }  // PK

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Platform { get; set; }

        public DateTime ReleaseDate { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        // Navigation property - One-to-Many (Game → Achievement)
        public ICollection<Achievement>? Achievements { get; set; }
    }
}