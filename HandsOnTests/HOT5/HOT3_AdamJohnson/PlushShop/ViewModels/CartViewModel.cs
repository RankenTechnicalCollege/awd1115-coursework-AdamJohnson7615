using System.Linq;
using System.Collections.Generic;

namespace PlushShop.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public int TotalQuantity => Items.Sum(i => i.Quantity);
        public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
    }
}
