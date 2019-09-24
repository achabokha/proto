using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Server.Controllers.Shop
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProductsController : Server.Controllers.BaseController
	{
		readonly Models.DbContext _ctx;

		public ProductsController(
			Models.DbContext ctx)
		{
			_ctx = ctx;
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			return await (from c in _ctx.Products
						  select c).ToListAsync();
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _ctx.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(string id, Product product)
		{
			if (id != product.Id)
			{
				return BadRequest();
			}

			_ctx.Entry(product).State = EntityState.Modified;

			try
			{
				await _ctx.SaveChangesAsync();
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

		[HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            var product = await _ctx.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _ctx.Products.Remove(product);
            await _ctx.SaveChangesAsync();

            return product;
        }

		private bool ProductExists(string id)
        {
            return _ctx.Products.Any(e => e.Id == id);
        }
	}
}