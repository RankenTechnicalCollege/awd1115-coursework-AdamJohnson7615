using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;

namespace StarLogger.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public PostController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Post/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            var myGames = await _context.UserGames
                .Where(ug => ug.UserId == user.Id)
                .Include(ug => ug.Game)
                .Select(ug => ug.Game)
                .OrderBy(g => g.Title)
                .ToListAsync();

            ViewBag.Games = myGames;
            return View();
        }

        // POST: /Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string content, int? gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Post cannot be empty.";
                var myGames = await _context.UserGames
                    .Where(ug => ug.UserId == user.Id)
                    .Include(ug => ug.Game)
                    .Select(ug => ug.Game)
                    .OrderBy(g => g.Title)
                    .ToListAsync();

                ViewBag.Games = myGames;
                return View();
            }

            var post = new Post
            {
                UserId = user.Id,
                Content = content,
                DatePosted = DateTime.Now,
                Type = PostType.StatusUpdate,
                GameId = gameId,
                LikeCount = 0
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Status updated!";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Post/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Game)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                // NEW: Include Likes and the Users who liked it
                .Include(p => p.PostLikes)
                    .ThenInclude(pl => pl.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            // Check if CURRENT user liked it (for the heart color)
            var userId = _userManager.GetUserId(User);
            ViewBag.IsLiked = userId != null && post.PostLikes.Any(pl => pl.UserId == userId);

            return View(post);
        }

        // POST: /Post/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Comment cannot be empty.";
                return RedirectToAction("Details", new { id = postId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            var comment = new Comment
            {
                PostId = postId,
                UserId = user.Id,
                Content = content,
                DatePosted = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = postId });
        }

        // GET: /Post/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var post = await _context.Posts.FindAsync(id);

            if (post == null) return NotFound();

            // STRICT SECURITY: Only the owner can edit. Admins will get 403 Forbidden.
            if (post.UserId != user.Id) return Forbid();

            var myGames = await _context.UserGames
                .Where(ug => ug.UserId == user.Id)
                .Include(ug => ug.Game)
                .Select(ug => ug.Game)
                .OrderBy(g => g.Title)
                .ToListAsync();

            ViewBag.Games = myGames;
            return View(post);
        }

        // POST: /Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string content, int? gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            var post = await _context.Posts.FindAsync(id);

            if (post == null) return NotFound();

            // STRICT SECURITY: Only the owner can edit
            if (post.UserId != user.Id) return Forbid();

            post.Content = content;
            post.GameId = gameId;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Post updated successfully!";
            return RedirectToAction("Index", "Home");
        }

        // POST: /Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var post = await _context.Posts.FindAsync(id);

            if (post == null) return NotFound();

            // MODERATION LOGIC: Allow if Owner OR Admin
            if (post.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Post deleted.";
            return RedirectToAction("Index", "Home");
        }
        // POST: /Post/ToggleLike
        [HttpPost]
        [Route("Post/ToggleLike")]
        public async Task<IActionResult> ToggleLike(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // 1. Check if user already liked this post
            var existingLike = await _context.PostLikes
                .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == user.Id);

            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return NotFound();

            bool isLikedNow;

            if (existingLike != null)
            {
                // REMOVE LIKE
                _context.PostLikes.Remove(existingLike);
                post.LikeCount--;
                isLikedNow = false;
            }
            else
            {
                // ADD LIKE
                var like = new PostLike { PostId = postId, UserId = user.Id };
                _context.PostLikes.Add(like);
                post.LikeCount++;
                isLikedNow = true;
            }

            // Safety check to prevent negative likes
            if (post.LikeCount < 0) post.LikeCount = 0;

            await _context.SaveChangesAsync();

            // Return the new data to the page
            return Json(new { success = true, newCount = post.LikeCount, isLiked = isLikedNow });
        }

    }
}