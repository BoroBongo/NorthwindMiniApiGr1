namespace ProductsApiApp.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public virtual ICollection<String> ProductsHtmlLinks { get; set; }


        public virtual string? Category { get; set; } // Add Category Name Only
        public string? CategoryLink { get; set; }
        public virtual string? Supplier { get; set; } // Add Supplier Name Only
        public string? SupplierLink { get; set; }
    }
}
