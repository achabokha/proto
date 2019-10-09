using System.Collections.Generic;

namespace Models.Entities.Shop
{
	public class Category: BaseEntity
	{
		public string Name { get; set; }
        public List<Product> Products {get ;set;}
	}
}