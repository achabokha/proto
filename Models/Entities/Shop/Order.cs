using System.Collections.Generic;

namespace Models.Entities.Shop
{
	public class Order: BaseEntity
	{
		
		public ApplicationUser User { get; set; }

		public EnumOrderStatus Status { get; set; }

		public virtual ICollection<OrderProducts> OrderProducts {get; set;}
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