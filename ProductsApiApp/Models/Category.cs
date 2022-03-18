using System;
using System.Collections.Generic;

namespace ProductsApiApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        /// <summary>
        /// This is the property for category ID which is a primary key
        /// </summary>
        public int CategoryId { get; set; } //
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? Picture { get; set; } //  <--------------------------- HERE SHAKIL

        public virtual ICollection<Product> Products { get; set; } // Don't return anything
    }
}
