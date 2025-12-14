using StarLogger.Models;

namespace StarLogger.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<UserGame> GameLibrary { get; set; }
        public List<Post> ActivityFeed { get; set; }

        public int GamesCount => GameLibrary.Count;
        public int ReviewsCount => ActivityFeed.Count(p => p.Type == PostType.ManualReview);
    }
}