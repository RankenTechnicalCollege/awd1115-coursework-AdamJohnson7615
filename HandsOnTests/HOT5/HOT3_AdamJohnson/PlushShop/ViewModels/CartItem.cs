namespace PlushShop.ViewModels
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string ImageFileName { get; set; } = "placeholder.png";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Slug { get; set; } = "";

        // Tracks how many are available in stock
        public int Stock { get; set; }
    }
}
