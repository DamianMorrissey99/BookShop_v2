using System.Collections.Generic;

namespace BookShop.Models.Interfaces
{
    public interface IBook
    {
        //string AuthorFirstName { get; set; }
        //string AuthorLastName { get; set; }
        //int BookId { get; set; }
        //string Genre { get; set; }
        //string ISBN { get; set; }
        //decimal Price { get; set; }
        //string Title { get; set; }
        IEnumerable<Book> Books { get;}
        Book GetBookById(int bookId); 
    }
}