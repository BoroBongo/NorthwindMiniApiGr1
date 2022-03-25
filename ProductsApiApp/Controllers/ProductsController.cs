#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApiApp.Models;

namespace ProductsApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;
        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var output = from product in _context.Products.Include(c => c.Supplier).Include(c => c.Category)
                        select new
                        {
                            product
                        };
            return await output.Select(x => ProductToDTO(x.product)).ToListAsync();
        }
        
        [HttpGet("sorted-by-categories")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCat()
        {
            var output = from product in _context.Products.Include(c => c.Supplier).Include(c => c.Category)
                         orderby product.Category.CategoryName
                         select product;
            return await output.Select(x => ProductToDTO(x)).ToListAsync();
        }
        
        [HttpGet("sorted-by-suppliers")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsBySuo()
        {
            var output = from product in _context.Products.Include(c => c.Supplier).Include(c => c.Category)
                         orderby product.Supplier.CompanyName
                         select product;
            return await output.Select(x => ProductToDTO(x)).ToListAsync();
        }
        
        [HttpGet("starts-with/{str}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsThatStartWith(string str)
        {
            str.ToLower();
            var output = from product in _context.Products.Include(c => c.Supplier).Include(c => c.Category)
                         where product.ProductName.ToLower().StartsWith(str)
                         select product;
            return await output.Select(x => ProductToDTO(x)).ToListAsync();
        }
        
        [HttpGet("price-lower/{price}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsLowerThan(int price)
        {
            var output = from product in _context.Products.Include(c => c.Supplier).Include(c => c.Category)
                         where product.UnitPrice <= price
                         select product;
            return await output.Select(x => ProductToDTO(x)).ToListAsync();
        }
        [HttpGet("price-higher/{price}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsHigherThan(int price)
        {
            var output = from product in _context.Products.Include(c => c.Supplier).Include(c => c.Category)
                         where product.UnitPrice >= price
                         select product;
            return await output.Select(x => ProductToDTO(x)).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.Include(c=>c.Supplier).Include(c=>c.Category).Where(c=>c.ProductId==id).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return ProductToDTO(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if (product.CategoryId != null)
            {

                product.Supplier = _context.Suppliers.Where(y => y.SupplierId.Equals(product.SupplierId)).FirstOrDefault();
                product.Category = _context.Categories.Where(x => x.CategoryId.Equals(product.CategoryId)).FirstOrDefault();
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        private static ProductDTO ProductToDTO(Product product) =>
            new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
                Category = product.Category.CategoryName,
                CategoryLink = $"/api/categories/{product.CategoryId}",
                Supplier = product.Supplier.CompanyName,
                SupplierLink = $"/api/suppliers/{product.SupplierId}",
                ProductsHtmlLinks = new List<string>{
                    $"/api/products/{product.ProductId}",
                    "/api/products/price-higher/{price}",
                    "/api/products/price-lower/{price}",
                    "/api/products/starts-with/{str}",
                    "/api/products/sorted-by-suppliers",
                    "/api/products/sorted-by-categories"
                },
            };
        
    }
}
