using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data
{
    public class ApplicationUser : IdentityUser
    {
       
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
  
        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Address Line 1 is required")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Display(Name = "Post Code")]
        [DataType(DataType.PostalCode)]
        public string PostCode { get; set; }
    }
}
