using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Application.IServices
{
    public interface IBookService
    {
        // Add a new book
        Task<Book> AddBook(Book book);

        // Update an existing book
        Task<Book> UpdateBook(Book book);

        // Delete a book by its ID
        Task<bool> DeleteBook(Guid id);

        // Get all books
        Task<IEnumerable<Book>> GetAllBooks();

        // Get a book by its ID
        Task<Book> GetBookById(Guid id);
        Task<Book?> GetBookByIdAsNoTracking(Guid id);
        Task<Book?> UpdateBookDetached(Book book);
    }
}
