namespace StarLogger.Models.ViewModels
{
    public class GameAchievementsViewModel
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }

        // Total number of achievements
        public int AchievementCount { get; set; }

        // List of achievements for this game
        public List<Achievement> Achievements { get; set; }
    }
}