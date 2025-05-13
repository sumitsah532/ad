using EcommerceBook.Application.Interfaces;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review> CreateReviewAsync(Guid userId, Guid bookId, int rating, string comment)
        {
            // Verify user has purchased the book
            var hasPurchased = await _reviewRepository.HasUserPurchasedBook(userId, bookId);
            if (!hasPurchased)
            {
                throw new UnauthorizedAccessException("You must purchase the book before reviewing it.");
            }

            var review = new Review(bookId, userId, rating, comment);
            return await _reviewRepository.AddAsync(review);
        }

        public async Task<bool> DeleteReviewAsync(Guid reviewId, Guid userId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null) return false;
            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own reviews.");
            }

            return await _reviewRepository.DeleteAsync(reviewId);
        }

        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            return await _reviewRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByBookIdAsync(Guid bookId)
        {
            return await _reviewRepository.GetByBookIdAsync(bookId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId)
        {
            return await _reviewRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> HasUserPurchasedBook(Guid userId, Guid bookId)
        {
            return await _reviewRepository.HasUserPurchasedBook(userId, bookId);
        }

        public async Task<Review> UpdateReviewAsync(Guid reviewId, Guid userId, int rating, string comment)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null) throw new ArgumentException("Review not found");
            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own reviews.");
            }

            review.UpdateReview(rating, comment);
            return await _reviewRepository.UpdateAsync(review);
        }
    }
}
