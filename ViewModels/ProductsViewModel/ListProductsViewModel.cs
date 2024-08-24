namespace ecommerce.ViewModels.ProductsViewModel
{
    public class ListProductsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; } = decimal.Round(0.00m, 2);
        public string Slug { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string Category { get; set; } = null!;
    }
}
