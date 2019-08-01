using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Components
{
    public class StoreCartSummary : ViewComponent
    {
        private readonly StoreCart _storeCart;

        public StoreCartSummary(StoreCart storeCart)
        {
            _storeCart = storeCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = _storeCart.GetCartItems();
            _storeCart.StoreCartItems = items;

            var storeCartViewModel = new StoreCartViewModel
            {
                StoreCart = _storeCart,
                StoreCartTotal = _storeCart.GetStoreCartTotalPrice()
            };

            return View(storeCartViewModel);
        }
        
    }
}
