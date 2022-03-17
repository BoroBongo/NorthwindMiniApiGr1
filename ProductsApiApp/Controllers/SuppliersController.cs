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
    public class SuppliersController : ControllerBase
    {
        private readonly ProductContext _context;

        public SuppliersController(ProductContext context)
        {
            _context = context;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliers()
        {
            return await _context.Suppliers
                .Select(x => SupplierToDTO(x))
                .ToListAsync();
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierProductsDTO>> GetSupplier(int id)
        {
            //var supplier = await _context.Suppliers.FindAsync(id);
            var supplier = await _context.Suppliers.Where(s => s.SupplierId == id).Include(s => s.Products).FirstOrDefaultAsync();

            if (supplier == null)
            {
                return NotFound();
            }

            return SupplierProductsToDTO(supplier);
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new { id = supplier.SupplierId }, supplier);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierId == id);
        }

        private static SupplierDTO SupplierToDTO(Supplier supplier) =>
            new SupplierDTO
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                ContactName = supplier.ContactName,
                ContactTitle = supplier.ContactTitle,
                Address = supplier.Address,
                City = supplier.City,
                Region = supplier.Region,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                HomePage = supplier.HomePage,
            };

        private static SupplierProductsDTO SupplierProductsToDTO(Supplier supplier) =>
            new SupplierProductsDTO
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                ContactName = supplier.ContactName,
                ContactTitle = supplier.ContactTitle,
                Address = supplier.Address,
                City = supplier.City,
                Region = supplier.Region,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                HomePage = supplier.HomePage,
                Products = supplier.Products.Select(p => p.ProductName).ToArray(),
                ProductsLinks = supplier.Products.Select(p => $"/api/products/{p.ProductId}").ToArray(),
            };
    }
}
