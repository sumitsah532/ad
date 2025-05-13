using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBook.Domain.IRepository
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetByBookIdAsync(Guid bookId);
        Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
        Task<bool> HasUserPurchasedBook(Guid userId, Guid bookId);
        Task<Review> AddAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(Guid id);
    }
}

