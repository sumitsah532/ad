using System;

namespace EcommerceBook.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Book Book { get; set; }

        // EF Core requires a parameterless constructor
        public OrderItem() { }

        public OrderItem(Guid bookId, int quantity, decimal price, string bookTitle)
        {
            Id = Guid.NewGuid();
            BookId = bookId;
            Quantity = quantity;
            Price = price;
            BookTitle = bookTitle;
        }

        // Optional individual setters if you prefer method-based updates
        public void SetOrderId(Guid orderId)
        {
            OrderId = orderId;
        }

        public void SetBookId(Guid bookId)
        {
            BookId = bookId;
        }

        public void SetBookTitle(string title)
        {
            BookTitle = title;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }

        public void SetOrder(Order order)
        {
            Order = order;
        }

        public void SetBook(Book book)
        {
            Book = book;
        }
    }
}
