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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BookStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly IOrderRepository _orderRepository;
        private readonly StoreCart _shoppingCart;


        public OrdersController(IOrderRepository orderRepository, StoreCart shoppingCart, UserManager<ApplicationUser> user, ApplicationDbContext dbContext)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _userManager = user;
            _context = dbContext;
        }


        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        public  IActionResult Delete()
        {
            return RedirectToAction("Index", "Books");
        }

        //[HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var userdetail = await _userManager.GetUserAsync(User);

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userdetail.Id);

            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.Address1 = user.AddressLine1;
            ViewBag.Address2 = user.AddressLine2;
            ViewBag.City = user.City;
            ViewBag.Country = user.Country;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.PostalCode = user.PostCode;
            ViewBag.Email = user.Email;

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
                var result = CheckTransUnionStatusAsync(order).Result;

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

            ViewBag.CheckoutCompleteMessage = "Thank you for your order.";
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

 
            var sessionId = HttpContext.Session.GetString("1916e0cd7ad540dd91d58ee5d69f660c");
            
            // TO DO : Get the correct Session ID
            //string sessionId = HttpContext.Session.Id;

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
