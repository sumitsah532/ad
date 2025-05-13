using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBook.Infrastructure.Repositories
{
    public class BookmarkDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } // Add what you need
        public DateTime CreatedAt { get; set; }
    }
    public class BookmarkRepository : IBookmarkRepository
    {
        private readonly BookStoreDbContext _context;

        public BookmarkRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Bookmark bookmark)
        {
            await _context.Bookmarks.AddAsync(bookmark);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid bookmarkId)
        {
            return await _context.Bookmarks.AnyAsync(b => b.Id == bookmarkId);
        }

        // ✅ Exists check by user and book
        public async Task<bool> ExistsAsync(Guid userId, Guid bookId)
        {
            return await _context.Bookmarks.AnyAsync(b => b.UserId == userId && b.BookId == bookId);
        }

        public async Task<List<Bookmark>> GetBookmarksByUserIdAsync(Guid userId)
        {
            return await _context.Bookmarks
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid bookmarkId)
        {
            var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(b => b.Id == bookmarkId);

            if (bookmark != null)
            {
                _context.Bookmarks.Remove(bookmark);
                await _context.SaveChangesAsync();
            }
        }

        // ✅ Delete by user and book
        public async Task DeleteByUserAndBookAsync(Guid userId, Guid bookId)
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId);

            if (bookmark != null)
            {
                _context.Bookmarks.Remove(bookmark);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBookMarkById(Guid bookMarkId)
        {

            var bookmark = await _context.Bookmarks
               .FirstOrDefaultAsync(b => b.Id == bookMarkId);

            if (bookmark != null)
            {
                _context.Bookmarks.Remove(bookmark);
                await _context.SaveChangesAsync();
            }
        }
    }

}
