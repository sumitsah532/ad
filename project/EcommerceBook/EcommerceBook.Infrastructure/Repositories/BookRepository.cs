using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _context;

        public BookRepository(BookStoreDbContext context)
        {
            _context = context;
        }


        
        public async Task<Book> AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> GetByIdAsNoTrackingAsync(Guid id)
        {
            return await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> UpdateDetachedAsync(Book book)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingBook = await _context.Books.FindAsync(book.Id);
                if (existingBook == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // Update all scalar properties
                _context.Entry(existingBook).CurrentValues.SetValues(book);

                // Handle special properties
                existingBook.SetBookImageUrl(book.BookImageUrl);
              //  existingBook.BookImageUrl = book.BookImageUrl;
                existingBook.Tags = book.Tags;

                // Handle sale information
                if (book.IsOnSale &&
                    book.SalePrice.HasValue &&
                    book.SaleStartDate.HasValue &&
                    book.SaleEndDate.HasValue)
                {
                    existingBook.SetSale(
                        book.SalePrice.Value,
                        book.SaleStartDate.Value,
                        book.SaleEndDate.Value
                    );
                }
                else
                {
                    existingBook.EndSale();
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return existingBook;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _context.Entry(book).State = EntityState.Detached; // Ensure entity is detached
                throw; // Re-throw to be handled by the service layer
            }
        }
    }
}