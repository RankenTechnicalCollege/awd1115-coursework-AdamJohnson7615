using System.ComponentModel.DataAnnotations;

namespace HandsOnTest1.Models
{
    public class OrderForm
    {
        private const decimal BasePrice = 15m;
        private const decimal TaxRate = 0.08m;

        [Required(ErrorMessage = "Please enter a quantity.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public string DiscountCode { get; set; } = ""; // optional

        public string DiscountMessage { get; set; } = "";

        public decimal DiscountPercent { get; set; } = 0;

        public decimal PricePerShirt => BasePrice * (1 - DiscountPercent / 100m);

        public decimal Subtotal => Quantity * PricePerShirt;

        public decimal Tax => Subtotal * TaxRate;

        public decimal Total => Subtotal + Tax;

        public void ApplyDiscountCode()
        {
            DiscountMessage = "";
            DiscountPercent = 0;

            switch (DiscountCode.Trim())
            {
                case "6175":
                    DiscountPercent = 30;
                    DiscountMessage = $"{DiscountPercent}% Discount Applied";
                    break;
                case "1390":
                    DiscountPercent = 20;
                    DiscountMessage = $"{DiscountPercent}% Discount Applied";
                    break;
                case "BB88":
                    DiscountPercent = 10;
                    DiscountMessage = $"{DiscountPercent}% Discount Applied";
                    break;
                case "":
                    DiscountPercent = 0;
                    DiscountMessage = "";
                    break;
                default:
                    DiscountPercent = 0;
                    DiscountMessage = "Invalid discount code.";
                    break;
            }
        }

    }
}
