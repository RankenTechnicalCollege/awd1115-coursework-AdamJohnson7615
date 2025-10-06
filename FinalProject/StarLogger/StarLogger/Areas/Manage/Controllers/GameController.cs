using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StarLogger.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GameController : Controller
    {
        private readonly StarLoggerContext _context;

        public GameController(StarLoggerContext context)
        {
            _context = context;
        }

        // GET: /manage/game/create
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Game";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Game"); // redirect to public list
            }
            return View(game);
        }

        // GET: /manage/game/edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            return View(game);
        }

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
                    if (!_context.Games.Any(e => e.GameId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Game");
            }
            return View(game);
        }

        // GET: /manage/game/delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            return View(game);
        }

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
            return RedirectToAction("Index", "Game");
        }
    }
}
