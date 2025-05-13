using EcommerceBook.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace EcommerceBook.Application.IServices
{
    public interface ICartService
    {
        Task<Cart> GetUserCartAsync(Guid userId);
        Task<Cart> AddItemToCartAsync(Guid userId, Guid bookId, int quantity, decimal price);
        Task<bool> RemoveItemFromCartAsync(Guid userId, Guid cartItemId);
        Task<bool> UpdateCartItemQuantityAsync(Guid userId, Guid cartItemId, int newQuantity);
        Task<bool> ClearCartAsync(Guid userId);
    }
}