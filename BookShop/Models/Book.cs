using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BookShop.Models.Interfaces;

namespace BookShop.Models
{
    public class Book : IBook
    {
        [Column("BookId")]
        public int BookId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Author First Name")]
        [StringLength(30, MinimumLength = 1)]
        [Required]
        public string AuthorFirstName { get; set; }

        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "Author Last Name")]
        [Required]
        public string AuthorLastName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string ISBN { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(30)]
        public string Genre { get; set; }

        [Required]
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public Book GetBookById(int bookId)
        {
            return Books.Where(x => x.BookId == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> Books { get; set; }
    }
}
