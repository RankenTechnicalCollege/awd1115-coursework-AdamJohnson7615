using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarLogger.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;
        public PostType Type { get; set; }
        public int LikeCount { get; set; } = 0;

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int? GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game? Game { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    }
}