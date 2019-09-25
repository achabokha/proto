using System.Collections.Generic;

namespace Models.Entities.Shop
{
	public class Product
	{
		public string Id { get; set; }
		public decimal Price { get; set; }  

        public string ImageUrl {get; set;}

        public Category Category {get; set;}
	}
}