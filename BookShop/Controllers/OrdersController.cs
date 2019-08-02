using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;
using BookShop.Data;
using BookShop.Interfaces;
using Trustev.Domain.Entities;
using Trustev.WebAsync;

namespace BookStore.Controllers
{
    public class OrdersController : Controller
    {

        private readonly IOrderRepository _orderRepository;
        private readonly StoreCart _shoppingCart;

        public OrdersController(IOrderRepository orderRepository, StoreCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }
        //private readonly ApplicationDbContext _context;

        //public OrdersController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        // GET: Orders
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Orders.ToListAsync());
        //}

        // GET: Orders/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders
        //        .FirstOrDefaultAsync(m => m.OrderId == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,OrderDate,OrderInCart,OrderComplete")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(order);
        //}

        // GET: Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(order);
        //}

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,OrderInCart,OrderComplete")] Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.OrderId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(order);
        //}

        // GET: Orders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders
        //        .FirstOrDefaultAsync(m => m.OrderId == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.OrderId == id);
        //}

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetCartItems();
            _shoppingCart.StoreCartItems = items;
            if (_shoppingCart.StoreCartItems.Count == 0)
            {
                ModelState.AddModelError("", "The cart is empty. Please add some books first");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.EmptyCart();

                // TO DO : not working ATM
                //var result =  CheckTransUnionStatusAsync(order).Result;

                //if (result)
                //{
                //    return RedirectToAction("CheckoutComplete");
                //}
                //else
                //{
                //    return RedirectToAction("CheckoutFailed");
                //}
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);

        }

        public IActionResult CheckoutComplete()
        {

            ViewBag.CheckoutCompleteMessage = "Thanks for your order.";
            return View();
        }

        public IActionResult CheckoutFailed()
        {

            ViewBag.CheckoutFailedMessage = "Your order cannot be completed at this time. Please contact your credit provider";
            return View();
        }

        private async Task<bool> CheckTransUnionStatusAsync(Order order)
        {         
            //TO DO: add to configuration
            ApiClient.SetUp("Tu-TestSite-test", "dc6647d7831948f88ac4e6436e67f178", "8a2407ba959f4c2cb8dd1fe5e9ee40a5", "EU");

            // TO DO : Get the correct Session ID
            string sessionId = HttpContext.Session.Id;

            Guid newGuid = Guid.Parse(sessionId);

            Random random = new Random();
            string nextNumber = random.Next().ToString();

            Case kase = new Case(newGuid, nextNumber);

            kase.Customer = new Customer()
            {
                FirstName = order.FirstName,
                LastName = order.LastName,
                PhoneNumber = order.PhoneNumber

            };

            Case returnCase = await ApiClient.PostCaseAsync(kase);
                        
            Decision decision = await ApiClient.GetDecisionAsync(returnCase.Id);    
    
            if (decision.Score == 1)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }
}
