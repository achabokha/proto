using System.Collections.Generic;

namespace Models.Entities.Shop
{
	public class Order
	{
		public string Id { get; set; }
		public List<Product> Products { get; set; }
		public ApplicationUser User { get; set; }

		public EnumOrderStatus status { get; set; }
	}

	public enum EnumOrderStatus
	{
		started,
		checkout,
		paid,
		processing,
		delivered
	}
}