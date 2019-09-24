using System.Collections.Generic;

namespace Models.Entities.Shop
{
    public class Product
    {
        public string Id {get; set;}
        public List<Category> Categories {get; set;}

        public decimal price;
    }
}