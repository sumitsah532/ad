using System;
using System.ComponentModel.DataAnnotations;

namespace EcommerceBook.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; private set; }

        [Range(1, 5)]
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Navigation properties
        public Guid BookId { get; private set; }
        public Book Book { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        // Constructor
        public Review(Guid bookId, Guid userId, int rating, string comment = null)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

            Id = Guid.NewGuid();
            BookId = bookId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void UpdateReview(int newRating, string newComment)
        {
            if (newRating < 1 || newRating > 5)
                throw new ArgumentOutOfRangeException(nameof(newRating), "Rating must be between 1 and 5.");

            Rating = newRating;
            Comment = newComment;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsEdited => UpdatedAt.HasValue;
    }
}