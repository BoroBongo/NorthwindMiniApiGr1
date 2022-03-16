namespace ProductsApiApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SuppId { get; set; }
        public int CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReordedLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}