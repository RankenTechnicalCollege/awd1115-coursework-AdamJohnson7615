namespace StarLogger.ViewModels
{
    public class GameSearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<GameResult> Results { get; set; } = new List<GameResult>();
    }

    public class GameResult
    {
        public string Title { get; set; }
        public string Platform { get; set; }
        public string CoverUrl { get; set; }
        public string IgdbId { get; set; }
    }
}