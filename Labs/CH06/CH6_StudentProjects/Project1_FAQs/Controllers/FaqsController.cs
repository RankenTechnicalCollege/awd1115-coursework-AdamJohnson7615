using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1_FAQs.Models;

namespace Project1_FAQs.Controllers
{
    public class FaqsController : Controller
    {
        private readonly FaqContext _context;

        public FaqsController(FaqContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? topicId, string? categoryId)
        {
            // Query FAQs including Topic and Category
            var faqs = _context.Faqs
                .Include(f => f.Topic)
                .Include(f => f.Category)
                .AsQueryable();

            // Apply topic filter if provided
            if (!string.IsNullOrEmpty(topicId))
                faqs = faqs.Where(f => f.TopicId == topicId);

            // Apply category filter if provided
            if (!string.IsNullOrEmpty(categoryId))
                faqs = faqs.Where(f => f.CategoryId == categoryId);

            // Pass data to the view
            ViewBag.Topics = _context.Topics.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedTopic = topicId;
            ViewBag.SelectedCategory = categoryId;

            return View(faqs.OrderBy(f => f.Id).ToList());
        }
    }
}
