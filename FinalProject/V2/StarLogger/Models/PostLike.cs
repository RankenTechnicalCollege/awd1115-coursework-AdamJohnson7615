using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarLogger.Models
{
    public class PostLike
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}