using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookShop.Data;

namespace BookShop.Models
{
    public class StoreCart
    {
        private readonly ApplicationDbContext _bookStoreContext;

        public StoreCart(ApplicationDbContext bookStoreContext)
        {
            _bookStoreContext = bookStoreContext;
        }

        public string StoreCartId { get;set;}
        public List<StoreCartItem> StoreCartItems { get; set; }

        public List<StoreCartItem> GetCartItems()
        {
            return StoreCartItems ??
                (StoreCartItems = _bookStoreContext.StoreCartItem.Where(x => x.StoreCartId == StoreCartId)
                .Include(y => y.Book).ToList());
        }

        public static StoreCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new StoreCart(context) { StoreCartId = cartId };
        }

        public decimal GetStoreCartTotalPrice()
        {
            var totalPrice = _bookStoreContext.StoreCartItem.Where(x => x.StoreCartId == StoreCartId)
                                        .Select(y => y.Book.Price * y.Quantity).Sum();
            return totalPrice;

        }

        public void AddBookTOCart(Book book, int amount)
        {
            var storeCartItem = _bookStoreContext.StoreCartItem
                    .SingleOrDefault(x => x.Book.BookId == book.BookId && x.StoreCartId == StoreCartId);

            if (storeCartItem == null)
            {
                storeCartItem = new StoreCartItem
                {
                    StoreCartId = StoreCartId,
                    Book = book,
                    Quantity = 1
                };

                _bookStoreContext.StoreCartItem.Add(storeCartItem);
            }
            else
            {
                storeCartItem.Quantity++;
            }
            _bookStoreContext.SaveChanges();
        }

        public int DeleteFromCart(Book book)
        {
            var storeCartItem = _bookStoreContext.StoreCartItem
                    .SingleOrDefault(x => x.Book.BookId == book.BookId && x.StoreCartId == StoreCartId);

            var tempQuantity = 0;

            if (storeCartItem != null)
            {
                if (storeCartItem.Quantity > 1)
                {
                    storeCartItem.Quantity--;
                    tempQuantity = storeCartItem.Quantity;
                }
                else
                {
                    _bookStoreContext.StoreCartItem.Remove(storeCartItem);
                }
            }
            _bookStoreContext.SaveChanges();
            return tempQuantity;
        }

        public void EmptyCart()
        {
            var cartItems = _bookStoreContext
                .StoreCartItem.Where(x => x.StoreCartId == StoreCartId);

            if (cartItems != null)
            {
                _bookStoreContext.StoreCartItem.RemoveRange(cartItems);
                _bookStoreContext.SaveChanges();
            }

        }
    }
}
