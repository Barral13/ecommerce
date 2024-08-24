using ecommerce.Models;
using System.Text.Json.Serialization;

namespace ecommerce.ViewModels.ProductsViewModel
{
    public class EditorProductViewModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; } = decimal.Round(0.00m, 2);
        public string Slug { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
