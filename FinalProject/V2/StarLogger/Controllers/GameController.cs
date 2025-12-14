using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StarLogger.Models;
using StarLogger.Repositories;
using StarLogger.Services;
using StarLogger.ViewModels;

namespace StarLogger.Controllers
{
    [Authorize] 
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepo;
        private readonly IIgdbService _igdbService;
        private readonly UserManager<User> _userManager;

        public GameController(IGameRepository gameRepo, IIgdbService igdbService, UserManager<User> userManager)
        {
            _gameRepo = gameRepo;
            _igdbService = igdbService;
            _userManager = userManager;
        }

        // GET: /Game/Index
        public async Task<IActionResult> Index(string search)
        {
            var model = new GameSearchViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                model.SearchTerm = search;
                model.Results = await _igdbService.SearchGamesAsync(search);
            }

            return View(model);
        }

        // POST: Perform Search (Manual form submission)
        [HttpPost]
        public async Task<IActionResult> Index(GameSearchViewModel model)
        {
            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                model.Results = await _igdbService.SearchGamesAsync(model.SearchTerm);
            }
            return View(model);
        }

        // GET: /Game/Details/5 (Real Database Entry)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var game = await _gameRepo.GetGameByIdAsync(id.Value);
            if (game == null) return NotFound();

            HttpContext.Session.SetString("LastViewedGame", game.Title);
            HttpContext.Session.SetInt32("LastViewedGameId", game.Id);

            var userId = _userManager.GetUserId(User);
            ViewBag.UserGame = await _gameRepo.GetUserGameAsync(userId, id.Value);

            return View(game);
        }

        // POST: /Game/Preview 
        [HttpPost]
        public async Task<IActionResult> Preview(string title, string platform, string igdbId, string coverUrl)
        {
            var existingGame = await _gameRepo.GetGameByIgdbIdAsync(igdbId);
            if (existingGame != null)
            {
                return RedirectToAction("Details", new { id = existingGame.Id });
            }

            var ghostGame = new Game
            {
                Id = 0,
                Title = title,
                Platform = platform,
                IgdbId = igdbId,
                CoverImageUrl = coverUrl,
                UserGames = new List<UserGame>()
            };

            HttpContext.Session.SetString("LastViewedGame", title);
            HttpContext.Session.Remove("LastViewedGameId"); 
            ViewBag.UserGame = null;
            return View("Details", ghostGame);
        }

        // POST: Add Game to Library
        [HttpPost]
        public async Task<IActionResult> Add(string title, string platform, string igdbId, string coverUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            var game = await _gameRepo.GetGameByIgdbIdAsync(igdbId);
            if (game == null)
            {
                game = new Game
                {
                    Title = title,
                    Platform = platform,
                    IgdbId = igdbId,
                    CoverImageUrl = coverUrl
                };
                await _gameRepo.AddGameAsync(game);
            }

            if (await _gameRepo.UserHasGameAsync(user.Id, game.Id))
            {
                TempData["Error"] = "You already have this game!";
            }
            else
            {
                var userGame = new UserGame
                {
                    UserId = user.Id,
                    GameId = game.Id,
                    Status = CompletionStatus.NotStarted,
                    DateAdded = DateTime.Now
                };
                await _gameRepo.AddUserGameAsync(userGame);
                TempData["Success"] = "Game added to library!";
            }

            return RedirectToAction("Index");
        }

        // GET: /Game/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            var userGame = await _gameRepo.GetUserGameAsync(user.Id, id);
            if (userGame == null) return NotFound();

            return View(userGame);
        }

        // POST: /Game/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int gameId, int? rating, CompletionStatus status, string achievements)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            var userGame = await _gameRepo.GetUserGameAsync(user.Id, gameId);
            if (userGame == null) return NotFound();

            userGame.Rating = rating;
            userGame.Status = status;
            userGame.AchievementsEarned = achievements;

            await _gameRepo.UpdateUserGameAsync(userGame);

            TempData["Success"] = "Game details updated!";
            return RedirectToAction("Index", "Profile", new { username = user.UserName });
        }
        // POST: /Game/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            var userGame = await _gameRepo.GetUserGameAsync(user.Id, gameId);

            if (userGame != null)
            {
                await _gameRepo.DeleteUserGameAsync(userGame);
                TempData["Success"] = "Game removed from library.";
            }
            else
            {
                TempData["Error"] = "Game not found in library.";
            }

            return RedirectToAction("Index", "Profile", new { username = user.UserName });
        }
    }
}