using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StarLogger.Data;
using StarLogger.Models;
using StarLogger.ViewModels;

namespace StarLogger.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // URL: /Profile/User
        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrEmpty(username)) return NotFound();

            var user = await _context.Users
                .Include(u => u.UserGames).ThenInclude(ug => ug.Game)
                .Include(u => u.Posts).ThenInclude(p => p.Game)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) return NotFound();

            var viewModel = new ProfileViewModel
            {
                User = user,
                GameLibrary = user.UserGames.OrderByDescending(ug => ug.DateAdded).ToList(),
                ActivityFeed = user.Posts.OrderByDescending(p => p.DatePosted).ToList()
            };

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != null)
            {
                ViewBag.LikedPostIds = await _context.PostLikes
                    .Where(pl => pl.UserId == currentUserId)
                    .Select(pl => pl.PostId)
                    .ToListAsync();
            }
            else
            {
                ViewBag.LikedPostIds = new List<int>();
            }

            return View(viewModel);
        }

        // POST: /Profile/UpdateAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Profile/UpdateAvatar")]
        public async Task<IActionResult> UpdateAvatar(string newUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            if (string.IsNullOrWhiteSpace(newUrl))
            {
                TempData["Error"] = "Avatar URL cannot be empty.";
                return RedirectToAction("Index", new { username = user.UserName });
            }

            user.ProfilePictureUrl = newUrl;
            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile picture updated!";
            return RedirectToAction("Index", new { username = user.UserName });
        }
    }
}