using System;
using System.Collections.Generic;
using System.Text;

namespace Project8
{
    public class Cart
    {
        // Fields
        private string _cartId;
        private Dictionary<string, double> _items;

        // Constructors
        public Cart()
        {
            _cartId = "Unknown";
            _items = new Dictionary<string, double>();
        }

        public Cart(string cartId)
        {
            _cartId = cartId;
            _items = new Dictionary<string, double>();
        }

        // Methods
        public void AddItem(string item, double price)
        {
            // If the item already exists, update its price instead of throwing an error
            _items[item] = price;
        }

        public void RemoveItem(string item)
        {
            _items.Remove(item);
        }

        public double GetTotal()
        {
            double total = 0;
            foreach (var item in _items)
            {
                total += item.Value;
            }
            return total;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Cart ID: {_cartId}");
            foreach (var item in _items)
            {
                result.AppendLine($"{item.Key}: ${item.Value:F2}");
            }
            result.AppendLine($"Total: ${GetTotal():F2}");
            return result.ToString();
        }
    }
}
