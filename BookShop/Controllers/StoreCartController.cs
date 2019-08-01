using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using BookShop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookShop
{
    public class StoreCartController : Controller
    {
        private readonly StoreCart _storeCart;
        private readonly IBook _book;

        public StoreCartController(IBook book, StoreCart storeCart)
        {
            _storeCart = storeCart;
            _book = book;
        }

        public ViewResult Index()
        {
            var items = _storeCart.GetCartItems();
            _storeCart.StoreCartItems = items;

            var storeCartVM = new StoreCartViewModel()
            {
                StoreCart = _storeCart,
                StoreCartTotal = _storeCart.GetStoreCartTotalPrice()
            };

            return View(storeCartVM);

        }
        public RedirectToActionResult AddToCart(int bookId)
        {            
            var selectedBook = _book.Books.FirstOrDefault(x => x.BookId == bookId);

            if (selectedBook != null)
            {
                _storeCart.AddBookTOCart(selectedBook, 1);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult DeleteFromStoreCart(int bookId)
        {
            var selectedBook = _book.Books.FirstOrDefault(x => x.BookId == bookId);

            if (selectedBook != null)
            {
                _storeCart.DeleteFromCart(selectedBook);
            }

            return RedirectToAction("Index");
        }
    }
}