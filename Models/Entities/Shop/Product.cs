using System.Collections.Generic;

namespace Models.Entities.Shop
{
	public class Product: BaseEntity
	{
		public decimal Price { get; set; }  
        public string ImageUrl {get; set;}
		public string Title {get ;set;}
		public string Description {get; set;}
        public Category Category {get; set;}

		public virtual ICollection<OrderProducts> OrderProducts {get; set;}
	}
}