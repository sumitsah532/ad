using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBook.Application.Interfaces
{
    public interface IReviewService
    {
        Task<Review> GetReviewByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetReviewsByBookIdAsync(Guid bookId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId);
        Task<Review> CreateReviewAsync(Guid userId, Guid bookId, int rating, string comment);
        Task<Review> UpdateReviewAsync(Guid reviewId, Guid userId, int rating, string comment);
        Task<bool> DeleteReviewAsync(Guid reviewId, Guid userId);
        Task<bool> HasUserPurchasedBook(Guid userId, Guid bookId);
    }
}