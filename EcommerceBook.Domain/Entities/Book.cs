using System;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceBook.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string ISBN { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get;  set; }

        public bool IsOnSale { get; private set; }
        public DateTime? SaleStartDate { get; private set; }
        public DateTime? SaleEndDate { get; private set; }
        public decimal? SalePrice { get; private set; }

        public string Category { get; private set; }
        public string BookImageUrl { get; private set; }

        public List<string> Tags { get; set; } = new List<string>();

        public ICollection<Review> Reviews { get; private set; } = new List<Review>();
        public ICollection<CartItem> CartItem { get; private set; } = new List<CartItem>();

        private Book() { } // For EF Core

        public Book(string title, string author, string isbn, decimal price, int stockQuantity, string category = null, string bookImageUrl = "", List<string> tags = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author cannot be empty.");
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("ISBN cannot be empty.");
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            if (stockQuantity < 0) throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock cannot be negative.");

            Id = Guid.NewGuid();
            Title = title;
            Author = author;
            ISBN = isbn;
            Price = price;
            StockQuantity = stockQuantity;
            Category = category;
            BookImageUrl = bookImageUrl ?? string.Empty;
            Tags = tags ?? new List<string>();  // Initialize tags

        }

        public string SetBookImageUrl(string bookImageUrl)
        {
            BookImageUrl = bookImageUrl;
            return BookImageUrl;
        }

        public void UpdateStock(int quantity)
        {
            StockQuantity += quantity;
        }

        public Guid SetGuid(Guid id)
        {
            Id = id;
            return Id;
        }

        public void SetSale(decimal salePrice, DateTime startDate, DateTime endDate)
        {
            if (salePrice <= 0 || salePrice >= Price)
                throw new ArgumentException("Sale price must be positive and less than the regular price.");
            if (endDate <= startDate)
                throw new ArgumentException("Sale end date must be after start date.");

            IsOnSale = true;
            SalePrice = salePrice;
            SaleStartDate = startDate;
            SaleEndDate = endDate;
        }

        public void EndSale()
        {
            IsOnSale = false;
            SalePrice = null;
            SaleStartDate = null;
            SaleEndDate = null;
        }

        public bool IsCurrentlyOnSale()
        {
            var now = DateTime.UtcNow;
            return IsOnSale && SaleStartDate <= now && SaleEndDate >= now;
        }

        public void SetTags(List<string> tags)
        {
            Tags = tags;
        }

        public void AddTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag) && !Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }
    }
}
