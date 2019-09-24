using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Shop;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers.Shop
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : Server.Controllers.BaseController
	{
		readonly Models.DbContext _ctx;

		public CategoriesController(
			Models.DbContext ctx)
		{
			_ctx = ctx;
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
		{
			return await (from c in _ctx.Categories
						  select c).ToListAsync();
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var category = await _ctx.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(string id, Category category)
		{
			if (id != category.Id)
			{
				return BadRequest();
			}

			_ctx.Entry(category).State = EntityState.Modified;

			try
			{
				await _ctx.SaveChangesAsync();
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

		[HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(string id)
        {
            var category = await _ctx.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _ctx.Categories.Remove(category);
            await _ctx.SaveChangesAsync();

            return category;
        }

		private bool CategoryExists(string id)
        {
            return _ctx.Categories.Any(e => e.Id == id);
        }
	}
}