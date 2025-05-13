using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Application.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBook.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> AddBook(Book book)
        {
            return await _bookRepository.AddAsync(book);
        }

        // Original update method (keep for compatibility)
        public async Task<Book> UpdateBook(Book book)
        {
            return await _bookRepository.UpdateAsync(book);
        }

        // New method for detached updates
        public async Task<Book> UpdateBookDetached(Book book)
        {
            // First get the existing book without tracking
            var existingBook = await _bookRepository.GetByIdAsNoTrackingAsync(book.Id);
            if (existingBook == null)
                return null;

            // Then perform the update with proper state management
            return await _bookRepository.UpdateDetachedAsync(book);
        }

        // New method to get book without tracking
        public async Task<Book> GetBookByIdAsNoTracking(Guid id)
        {
            return await _bookRepository.GetByIdAsNoTrackingAsync(id);
        }

        public async Task<bool> DeleteBook(Guid id)
        {
            return await _bookRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookById(Guid id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }
    }
}