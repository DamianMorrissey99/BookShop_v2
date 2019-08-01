using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class StoreCartItem
    {
        public int StoreCartItemId { get; set; }
        public string StoreCartId { get; set; }
        public Book Book {get; set;}
        public int Quantity { get; set; }
        
    }
}
