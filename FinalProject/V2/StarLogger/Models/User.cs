using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StarLogger.Models
{
    public class User : IdentityUser
    {
        public string? ProfilePictureUrl { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}