using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreDbContext _context;

        public OrderRepository(BookStoreDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Book>> GetAllBooksHistory(Guid userId)
        {
            var orders = await _context.Orders
                .Where(order => order.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .ToListAsync();

            var books = orders
                .SelectMany(order => order.OrderItems)
                .Select(oi => oi.Book)
                .Distinct()
                .ToList();

            return books;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            // Generate claim code
            order.SetClaimCode(await GenerateClaimCodeInternalAsync());

            // Set order items
            foreach (var item in order.OrderItems)
            {
                item.OrderId = order.Id;
                await _context.OrderItems.AddAsync(item);
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.SetStatus(status);

            if (status == OrderStatus.Completed && !order.IsFulfilled)
            {
                order.SetIsFulfilled(true);
                order.SetFulfilledDate(DateTime.UtcNow);

                var user = await _context.Users.FindAsync(order.UserId);
                if (user != null)
                {
                    user.IncrementSuccessfulOrders();
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> FulfillOrderAsync(string claimCode)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.ClaimCode == claimCode && !o.IsFulfilled);

            if (order == null) return false;

            order.SetIsFulfilled(true);
            order.SetStatus(OrderStatus.Completed);
            order.SetFulfilledDate(DateTime.UtcNow);

            var user = await _context.Users.FindAsync(order.UserId);
            if (user != null)
            {
                user.IncrementSuccessfulOrders();
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.Status == OrderStatus.Completed) return false;

            order.SetStatus(OrderStatus.Cancelled);
            order.SetCancelledDate(DateTime.UtcNow);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<decimal> CalculateDiscountAsync(Guid userId, int bookCount)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 0m;

            decimal discount = 0m;
            if (bookCount >= 5) discount += 0.05m;
            if (user.SuccessfulOrders >= 10) discount += 0.10m;

            return discount;
        }

        public async Task<bool> OrderExistsAsync(Guid orderId)
        {
            return await _context.Orders.AnyAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersByUserAsync(Guid userId, int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Where(o => o.UserId == userId && o.OrderDate >= cutoffDate)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        private async Task<string> GenerateClaimCodeInternalAsync()
        {
            string code;
            do
            {
                code = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12).ToUpper();
            }
            while (await _context.Orders.AnyAsync(o => o.ClaimCode == code));

            return code;
        }

        public Task GenerateClaimCodeAsync()
        {
            throw new NotImplementedException();
        }
    }
}