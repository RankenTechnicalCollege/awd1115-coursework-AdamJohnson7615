using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlushShop.Data;
using PlushShop.Helpers;
using PlushShop.Models;
using PlushShop.ViewModels;

namespace PlushShop.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _ctx;
        private const string CartSessionKey = "cart-v1";

        public CartController(AppDbContext ctx) => _ctx = ctx;

        // View Cart
        [HttpGet("/cart/")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<CartViewModel>(CartSessionKey) ?? new CartViewModel();
            return View(cart);
        }

        // Add to Cart
        [HttpPost("/cart/add/{slug}/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(string slug, int qty = 1)
        {
            var product = await _ctx.Products.FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObject<CartViewModel>(CartSessionKey) ?? new CartViewModel();

            var existing = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing == null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageFileName = product.ImageFileName,
                    Price = product.Price,
                    Quantity = Math.Min(qty, product.Stock),
                    Slug = product.Slug,
                    Stock = product.Stock
                });
            }
            else
            {
                existing.Quantity = Math.Min(existing.Quantity + qty, product.Stock);
                existing.Stock = product.Stock;
            }

            HttpContext.Session.SetObject(CartSessionKey, cart);
            TempData["Message"] = $"{product.Name} added to cart.";
            return RedirectToAction("Index");
        }

        // Remove from Cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObject<CartViewModel>(CartSessionKey) ?? new CartViewModel();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                HttpContext.Session.SetObject(CartSessionKey, cart);
                TempData["Message"] = $"{item.ProductName} removed from cart.";
            }
            return RedirectToAction("Index");
        }

        // Update quantity in Cart (stock-aware)
        [HttpPost("/cart/update/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int productId, int qty)
        {
            var cart = HttpContext.Session.GetObject<CartViewModel>(CartSessionKey) ?? new CartViewModel();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                var product = await _ctx.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product != null)
                {
                    item.Quantity = Math.Min(qty, product.Stock);
                    item.Stock = product.Stock;
                }

                HttpContext.Session.SetObject(CartSessionKey, cart);
                TempData["Message"] = $"Updated quantity for {item.ProductName}.";
            }
            return RedirectToAction("Index");
        }

        // Checkout
        [HttpPost("/cart/checkout/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(string customerName = "Guest")
        {
            var cart = HttpContext.Session.GetObject<CartViewModel>(CartSessionKey);
            if (cart == null || !cart.Items.Any())
            {
                TempData["Message"] = "Cart is empty.";
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                CustomerName = customerName,
                Total = cart.TotalPrice,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var it in cart.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = it.ProductId,
                    ProductName = it.ProductName,
                    Price = it.Price,
                    Quantity = it.Quantity
                });

                // Reduce stock
                var product = await _ctx.Products.FirstOrDefaultAsync(p => p.Id == it.ProductId);
                if (product != null)
                {
                    product.Stock -= it.Quantity;
                    if (product.Stock < 0) product.Stock = 0;
                }
            }

            _ctx.Orders.Add(order);
            await _ctx.SaveChangesAsync();

            // Clear cart
            HttpContext.Session.Remove(CartSessionKey);
            TempData["Message"] = "Purchase complete! Thank you.";

            return RedirectToAction("Index");
        }
    }
}
