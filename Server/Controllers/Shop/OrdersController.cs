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
	public class OrdersController : Server.Controllers.BaseController
	{
		readonly Models.DbContext _ctx;

		public OrdersController(
			Models.DbContext ctx)
		{
			_ctx = ctx;
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			return await (from c in _ctx.Orders
						  select c).ToListAsync();
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _ctx.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(string id, Order order)
		{
			if (id != order.Id)
			{
				return BadRequest();
			}

			_ctx.Entry(order).State = EntityState.Modified;

			try
			{
				await _ctx.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(id))
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
        public async Task<ActionResult<Order>> DeleteOrder(string id)
        {
            var order = await _ctx.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _ctx.Orders.Remove(order);
            await _ctx.SaveChangesAsync();

            return order;
        }

		private bool OrderExists(string id)
        {
            return _ctx.Orders.Any(e => e.Id == id);
        }
	}
}