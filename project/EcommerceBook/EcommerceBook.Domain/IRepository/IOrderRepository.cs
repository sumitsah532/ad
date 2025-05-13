using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Domain.Repositories
{
    public interface IOrderRepository
    {
        // Basic Order Operations
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<Order> CreateOrderAsync(Order order);

        // Status Management
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<bool> FulfillOrderAsync(string claimCode);
        Task<bool> CancelOrderAsync(Guid orderId);

        // Administrative Operations
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);

        // Business Logic
        Task<decimal> CalculateDiscountAsync(Guid userId, int bookCount);

        // Additional Helpers (recommended)
        Task<bool> OrderExistsAsync(Guid orderId);
        Task GenerateClaimCodeAsync();
        Task<IEnumerable<Order>> GetRecentOrdersByUserAsync(Guid userId, int days);
        Task<List<Book>> GetAllBooksHistory(Guid userId);
    }
}