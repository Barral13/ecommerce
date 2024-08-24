using System.Text.Json.Serialization;

namespace ecommerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        [JsonIgnore]
        public IList<Product> Products { get; set; } = null!;
    }
}
