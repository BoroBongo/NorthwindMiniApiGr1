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
    public class CategoriesController : ControllerBase
    {
        private readonly ProductContext _context;

        public CategoriesController(ProductContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            return await _context.Categories
                .Select(x => CategoryToDTO(x))
                .ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryProductsDTO>> GetCategory(int id)
        {
            //var category = await _context.Categories.FindAsync(id);
            var category = await _context.Categories.Where(c => c.CategoryId == id).Include(c => c.Products).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return CategoryProductsToDTO(category);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        private static CategoryDTO CategoryToDTO(Category category) =>
            new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
            };

        private static CategoryProductsDTO CategoryProductsToDTO(Category category) =>
            new CategoryProductsDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Products = category.Products.Select(p=>p.ProductName).ToArray(),
                ProductsLinks = category.Products.Select(p => $"/api/products/{p.ProductId}").ToArray(),
            };
    }
}
