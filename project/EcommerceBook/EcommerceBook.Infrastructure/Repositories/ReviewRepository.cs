using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Interfaces;
using EcommerceBook.Domain.IRepository;
using EcommerceBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookStoreDbContext _context;

        public ReviewRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Review> GetByIdAsync(Guid id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetByBookIdAsync(Guid bookId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Reviews
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> HasUserPurchasedBook(Guid userId, Guid bookId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .AnyAsync(oi => oi.BookId == bookId && oi.Order.UserId == userId);
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}