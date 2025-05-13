using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EcommerceBook.Domain.Interfaces
{
    public interface IOrderService
    {
        // Order operations
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<Order> CreateOrderAsync(Guid userId, IEnumerable<CartItem> items);

        // Status management
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<bool> FulfillOrderAsync(string claimCode);
        Task<bool> CancelOrderAsync(Guid orderId);

        // Administrative operations
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);

        // Business logic
        Task<decimal> CalculateOrderDiscountAsync(Guid userId, int bookCount);

        // Helpers
        Task<bool> OrderExistsAsync(Guid orderId);
        Task<IEnumerable<Order>> GetRecentOrdersByUserAsync(Guid userId, int days);
        Task<IEnumerable<Book>> GetAllOrderBooks(Guid userId);
    }
}