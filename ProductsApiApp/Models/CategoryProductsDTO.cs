namespace ProductsApiApp.Models
{
    public class CategoryProductsDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<String> Products { get; set; }
        public virtual ICollection<String> ProductsLinks { get; set; }
    }
}
