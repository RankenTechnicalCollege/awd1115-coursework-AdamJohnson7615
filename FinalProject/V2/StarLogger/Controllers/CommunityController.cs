using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;

namespace StarLogger.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommunityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Community
        public async Task<IActionResult> Index()
        {
            var communityMembers = await _context.Users
                .Select(u => new CommunityMemberViewModel
                {
                    UserName = u.UserName,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    GameCount = _context.UserGames.Count(ug => ug.UserId == u.Id)
                })
                .OrderByDescending(m => m.GameCount)
                .ToListAsync();

            return View(communityMembers);
        }
    }

    // Helper
    public class CommunityMemberViewModel
    {
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int GameCount { get; set; }
    }
}