using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
     //   public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BookShop.Models.Book> Book { get; set; }
        
        public DbSet<BookShop.Models.StoreCartItem> StoreCartItem { get; set; }

        public DbSet<BookShop.Models.Order> Orders {  get; set; }
        public DbSet<BookShop.Models.OrderBooks> OrderBooks { get; set; }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
    }
}
