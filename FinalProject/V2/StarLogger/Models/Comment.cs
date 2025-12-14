using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarLogger.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post? Post { get; set; }
    }
}