using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlushShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string CustomerName { get; set; } = "Guest";

        [Required]
        public decimal Total { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
