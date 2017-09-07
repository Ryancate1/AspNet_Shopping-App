using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rcateShoppingApp1.Models.CodeFirst
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string CustomerId { get; set; }
        public int Count { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual Item Item { get; set; }
        public virtual ApplicationUser Customer { get; set; }

        public decimal? UnitTotal
        {
            get
            {
                return Count * Item.Price;
            }
        }
    }
}