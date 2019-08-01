using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }
}
