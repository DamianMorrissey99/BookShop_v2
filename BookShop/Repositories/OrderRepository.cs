using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Data;
using BookShop.Interfaces;
using BookShop.Models;

namespace BookShop.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly StoreCart _cart;


        public OrderRepository(ApplicationDbContext appDbContext, StoreCart storeCart)
        {
            _appDbContext = appDbContext;
            _cart = storeCart;
        }

        public void CreateOrder(Order order)
        {
            order.OrderPlacedDate = DateTime.Now;

            _appDbContext.Orders.Add(order);

            var cartItems = _cart.StoreCartItems;

            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderBooks()
                {
                    Quantity = cartItem.Quantity,
                    BookId = cartItem.Book.BookId,
                    OrderId = order.OrderId,
                    Price = cartItem.Book.Price
                };

                _appDbContext.OrderBooks.Add(orderDetail);
            }

            _appDbContext.SaveChanges();
        }
    }
}
