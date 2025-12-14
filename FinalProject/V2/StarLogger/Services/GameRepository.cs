using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;

namespace StarLogger.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _context.Games
                .Include(g => g.UserGames)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game?> GetGameByIgdbIdAsync(string igdbId)
        {
            return await _context.Games
                .Include(g => g.UserGames)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.IgdbId == igdbId);
        }

        public async Task AddGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task<UserGame?> GetUserGameAsync(string userId, int gameId)
        {
            return await _context.UserGames
                .Include(ug => ug.Game)
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);
        }

        public async Task AddUserGameAsync(UserGame userGame)
        {
            _context.UserGames.Add(userGame);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserGameAsync(UserGame userGame)
        {
            _context.UserGames.Update(userGame);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserGameAsync(UserGame userGame)
        {
            _context.UserGames.Remove(userGame);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserHasGameAsync(string userId, int gameId)
        {
            return await _context.UserGames.AnyAsync(ug => ug.UserId == userId && ug.GameId == gameId);
        }
    }
}