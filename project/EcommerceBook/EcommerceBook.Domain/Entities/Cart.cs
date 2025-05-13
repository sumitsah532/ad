namespace EcommerceBook.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public bool IsActive { get; private set; } = true; // Is the cart active?

        // Navigation property
        public User User { get; private set; }
        public ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();

        // Constructor
        public Cart(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CreatedDate = DateTime.UtcNow;
        }

        // Methods to interact with CartItems
        public void AddItem(CartItem cartItem)
        {
            CartItems.Add(cartItem);
        }

        public void RemoveItem(CartItem cartItem)
        {
            CartItems.Remove(cartItem);
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }

        public void UpdateCartStatus(bool cartCartStatus)
        {
            IsActive = cartCartStatus;
        }
    }
}
