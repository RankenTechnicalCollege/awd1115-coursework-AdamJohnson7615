using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;
using StarLogger.ViewModels;

namespace StarLogger.Areas.Admin.Controllers
{
    [Area("Admin")] // <--- This connects it to /Admin/Users
    [Authorize(Roles = "Admin")] // <--- Security: Only Admins can enter
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context; // Needed for manual cleanup

        public UsersController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfilePictureUrl = user.ProfilePictureUrl ?? "https://via.placeholder.com/150",
                    IsAdmin = roles.Contains("Admin")
                });
            }

            return View(model);
        }

        // POST: /Admin/Users/ToggleRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // PROTECTION: Cannot touch StarCommander
            if (user.UserName == "StarCommander")
            {
                TempData["Error"] = "You cannot modify the Supreme Commander.";
                return RedirectToAction("Index");
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                TempData["Success"] = $"Demoted {user.UserName} to regular user.";
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["Success"] = $"Promoted {user.UserName} to Admin.";
            }

            return RedirectToAction("Index");
        }

        // POST: /Admin/Users/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // PROTECTION: Cannot delete StarCommander
            if (user.UserName == "StarCommander")
            {
                TempData["Error"] = "You cannot delete the Supreme Commander.";
                return RedirectToAction("Index");
            }

            // 1. Manually remove PostLikes to prevent SQL errors 
            // (Since we set DeleteBehavior.NoAction earlier)
            var userLikes = _context.PostLikes.Where(pl => pl.UserId == userId);
            _context.PostLikes.RemoveRange(userLikes);
            await _context.SaveChangesAsync();

            // 2. Now delete the user (EF Core will cascade delete their Games & Posts)
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "User deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Error deleting user.";
            }

            return RedirectToAction("Index");
        }
    }
}