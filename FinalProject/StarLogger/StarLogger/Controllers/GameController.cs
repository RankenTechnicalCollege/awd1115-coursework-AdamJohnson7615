using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;
using StarLogger.Helpers;
using System.Linq;
using System.Threading.Tasks;
using StarLogger.Models.ViewModels;

namespace StarLogger.Controllers
{
    public class GameController : Controller
    {
        private readonly StarLoggerContext _context;

        public GameController(StarLoggerContext context)
        {
            _context = context;
        }

        // GET: /games/
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games.ToListAsync();
            ViewData["Title"] = "Games";
            return View(games);
        }

        // GET: /games/{id}/{slug}/
        public async Task<IActionResult> Details(int? id, string slug)
        {
            if (id == null) return NotFound();

            var game = await _context.Games
                .Include(g => g.Achievements)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game == null) return NotFound();

            // Set page title
            ViewData["Title"] = game.Title;

            // Redirect if slug doesn't match
            var generatedSlug = SlugHelper.ToSlug(game.Title);
            if (slug != generatedSlug)
                return RedirectToAction("Details", new { id, slug = generatedSlug });

            return View(game);
        }

        // GET: /games/create/
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Game";
            return View();
        }

        // POST: /games/create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = "Create Game";
            return View(game);
        }

        // GET: /games/edit/{id}/{slug}/
        public async Task<IActionResult> Edit(int? id, string slug)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            ViewData["Title"] = $"Edit {game.Title}";
            return View(game);
        }

        // POST: /games/edit/{id}/{slug}/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.GameId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = $"Edit {game.Title}";
            return View(game);
        }

        // GET: /games/delete/{id}/{slug}/
        public async Task<IActionResult> Delete(int? id, string slug)
        {
            if (id == null) return NotFound();

            var game = await _context.Games
                .Include(g => g.Achievements)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game == null) return NotFound();

            ViewData["Title"] = $"Delete {game.Title}";
            return View(game);
        }

        // POST: /games/delete/{id}/{slug}/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> WithAchievements()
        {
            var data = await _context.Games
                .Include(g => g.Achievements)
                .Select(g => new GameAchievementsViewModel
                {
                    GameId = g.GameId,
                    Title = g.Title,
                    Platform = g.Platform,
                    Genre = g.Genre,
                    ReleaseDate = g.ReleaseDate,
                    AchievementCount = g.Achievements.Count,
                    Achievements = g.Achievements.ToList()
                })
                .ToListAsync();

            ViewData["Title"] = "Games & Achievements";
            return View(data);
        }

        private bool GameExists(int id) => _context.Games.Any(e => e.GameId == id);
    }
}
