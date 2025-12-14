using StarLogger.Models;

namespace StarLogger.Repositories
{
    public interface IGameRepository
    {
        Task<Game?> GetGameByIdAsync(int id);
        Task<Game?> GetGameByIgdbIdAsync(string igdbId);
        Task AddGameAsync(Game game);

        Task<UserGame?> GetUserGameAsync(string userId, int gameId);
        Task AddUserGameAsync(UserGame userGame);
        Task UpdateUserGameAsync(UserGame userGame);
        Task<bool> UserHasGameAsync(string userId, int gameId);
        Task DeleteUserGameAsync(UserGame userGame);
    }
}