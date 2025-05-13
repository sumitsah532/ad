namespace EcommerceBook.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; }
        public Guid CartId { get; private set; }
        public Guid BookId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        // Navigation properties
        public Cart Cart { get; private set; }
        public Book Book { get; private set; }

        // EF Core requires a parameterless constructor
        protected CartItem() { }

        public CartItem(Guid cartId, Guid bookId, int quantity, decimal price)
        {
            Id = Guid.NewGuid();
            CartId = cartId;
            BookId = bookId;
            Quantity = quantity;
            Price = price;
        }
        public CartItem( Guid bookId, int quantity)
        {
            BookId = bookId;
            Quantity = quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");
            Quantity = quantity;
        }

        public void UpdatePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Price cannot be negative");
            Price = price;
        }
    }
}