using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Interfaces;
using EcommerceBook.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ICartRepository cartRepository,
            IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            return order;
        }

        public async Task<IEnumerable<Book>> GetAllOrderBooks(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");
            var books= await _orderRepository.GetAllBooksHistory(userId);
            return books;


        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            var user = await _userRepository.GetByIdAsync(userId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order> CreateOrderAsync(Guid userId, IEnumerable<CartItem> cartItems)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            if (cartItems == null || !cartItems.Any())
                throw new ArgumentException("Order must contain at least one item");

            // Verify user exists
            var user = await _userRepository.GetByIdAsync(userId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // Get active cart
            var activeCart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (activeCart == null)
                throw new KeyNotFoundException("Active cart not found");

            // Verify all cart items exist and have sufficient stock
            var verifiedItems = new List<CartItem>();
            foreach (var cartItem in cartItems)
            {
                var book = await _bookRepository.GetByIdAsync(cartItem.BookId);
                if (book == null)
                    throw new KeyNotFoundException($"Book {cartItem.BookId} not found");

                if (book.StockQuantity < cartItem.Quantity)
                    throw new InvalidOperationException($"Not enough stock for book {book.Title}");

                // Ensure we're using the current price
                cartItem.UpdatePrice(book.IsOnSale && book.SalePrice.HasValue
                    ? book.SalePrice.Value
                    : book.Price);

                verifiedItems.Add(cartItem);
            }

            // Create the order
            var order = Order.Create(
                userId,
                verifiedItems,
                user.SuccessfulOrders,
                () => Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper());

            // Deactivate cart
            activeCart.UpdateCartStatus(false);
            await _cartRepository.UpdateCartAsync(activeCart);

            // Reduce book quantities
            foreach (var cartItem in verifiedItems)
            {
                var book = await _bookRepository.GetByIdAsync(cartItem.BookId);
                book.StockQuantity -= cartItem.Quantity;
                await _bookRepository.UpdateAsync(book);
            }

            return await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty");

            if (!await _orderRepository.OrderExistsAsync(orderId))
                throw new KeyNotFoundException("Order not found");

            return await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        }

        public async Task<bool> FulfillOrderAsync(string claimCode)
        {
            if (string.IsNullOrWhiteSpace(claimCode))
                throw new ArgumentException("Claim code cannot be empty");

            return await _orderRepository.FulfillOrderAsync(claimCode);
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            if (order.Status == OrderStatus.Completed)
                throw new InvalidOperationException("Completed orders cannot be cancelled");

            // Restore book quantities if order is cancelled
            if (order.Status != OrderStatus.Cancelled)
            {
                foreach (var item in order.OrderItems)
                {
                    var book = await _bookRepository.GetByIdAsync(item.Book.Id);
                    if (book != null)
                    {
                        book.UpdateStock( item.Quantity);
                        await _bookRepository.UpdateAsync(book);
                    }
                }
            }

            return await _orderRepository.CancelOrderAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _orderRepository.GetOrdersByStatusAsync(status);
        }

        public async Task<decimal> CalculateOrderDiscountAsync(Guid userId, int bookCount)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            if (bookCount <= 0)
                throw new ArgumentException("Book count must be positive");

            var user = await _userRepository.GetByIdAsync(userId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _orderRepository.CalculateDiscountAsync(userId, bookCount);
        }

        public async Task<bool> OrderExistsAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty");

            return await _orderRepository.OrderExistsAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersByUserAsync(Guid userId, int days)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            if (days <= 0)
                throw new ArgumentException("Days must be positive");

            var user = await _userRepository.GetByIdAsync(userId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _orderRepository.GetRecentOrdersByUserAsync(userId, days);
        }
    }
}