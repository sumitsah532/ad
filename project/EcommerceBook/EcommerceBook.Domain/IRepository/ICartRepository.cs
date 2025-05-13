using EcommerceBook.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace EcommerceBook.Domain.Repositories
{
    public interface ICartRepository
    {
        // Cart methods
        Task<Cart> GetCartByUserIdAsync(Guid userId);
        Task<Cart> GetCartByIdAsync(Guid cartId);
        Task<Cart> AddCartAsync(Cart cart);  // Changed to return Task<Cart>
        Task UpdateCartAsync(Cart cart);
        Task<bool> DeleteCartAsync(Guid cartId);  // Returns boolean to indicate success

        // CartItem methods
        Task<CartItem> GetCartItemByIdAsync(Guid cartItemId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);  // Changed to return Task<CartItem> for clarity
        Task UpdateCartItemAsync(CartItem cartItem);
        Task<bool> DeleteCartItemAsync(Guid cartItemId);  // Returns boolean to indicate success
        Task<bool> ClearCartItemsAsync(Guid cartId);
    }
}
