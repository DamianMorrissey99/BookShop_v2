using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Data;
using BookShop.Models;
using BookShop.Models.Interfaces;

namespace BookShop.Repositories
{
    public class BookRepository : IBook
    {
        private readonly ApplicationDbContext _appDbContext;

        public BookRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Book> Books => _appDbContext.Book;

        public Book GetBookById(int bookId) => _appDbContext.Book.FirstOrDefault(p => p.BookId == bookId);
    }
}
