using System;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceBook.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal TotalPrice { get; private set; }
        public decimal DiscountTotal { get; private set; }
        public string ClaimCode { get; private set; }
        public bool IsFulfilled { get; private set; }
        public DateTime? FulfilledDate { get; private set; }
        public DateTime? CancelledDate { get; private set; }

        // Navigation properties
        public User User { get; private set; }
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        // EF Core requires a parameterless constructor
        protected Order() { }

        public static Order Create(
            Guid userId,
            IEnumerable<CartItem> cartItems,
            int userSuccessfulOrders,
            Func<string> claimCodeGenerator)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                IsFulfilled = false,
                ClaimCode = claimCodeGenerator()
            };

            foreach (var cartItem in cartItems)
            {
                order.AddOrderItem(new OrderItem(
                    cartItem.BookId,
                    cartItem.Quantity,
                    cartItem.Price,
                    cartItem.Book?.Title ?? string.Empty));
            }

            var totalBeforeDiscount = order.OrderItems.Sum(i => i.Price * i.Quantity);
            order.DiscountTotal = order.CalculateDiscounts(userSuccessfulOrders, totalBeforeDiscount);
            order.TotalPrice = totalBeforeDiscount - order.DiscountTotal;

            return order;
        }

        public void AddOrderItem(OrderItem item)
        {
            _orderItems.Add(item);
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("Completed orders cannot be cancelled");

            Status = OrderStatus.Cancelled;
            CancelledDate = DateTime.UtcNow;
        }

        public void MarkAsFulfilled()
        {
            if (IsFulfilled)
                throw new InvalidOperationException("Order is already fulfilled");

            Status = OrderStatus.Completed;
            IsFulfilled = true;
            FulfilledDate = DateTime.UtcNow;
        }

        private decimal CalculateDiscounts(int userSuccessfulOrders, decimal totalBeforeDiscount)
        {
            decimal discount = 0;
            int totalBooks = OrderItems.Sum(i => i.Quantity);

            if (totalBooks >= 5)
                discount += totalBeforeDiscount * 0.05m;

            if (userSuccessfulOrders >= 10)
                discount += totalBeforeDiscount * 0.10m;

            return discount;
        }

        // --- Public Setter Methods ---
        public void SetId(Guid id) => Id = id;
        public void SetUserId(Guid userId) => UserId = userId;
        public void SetOrderDate(DateTime orderDate) => OrderDate = orderDate;
        public void SetStatus(OrderStatus status) => Status = status;
        public void SetTotalPrice(decimal totalPrice) => TotalPrice = totalPrice;
        public void SetDiscountTotal(decimal discountTotal) => DiscountTotal = discountTotal;
        public void SetClaimCode(string claimCode) => ClaimCode = claimCode;
        public void SetIsFulfilled(bool isFulfilled) => IsFulfilled = isFulfilled;
        public void SetFulfilledDate(DateTime? fulfilledDate) => FulfilledDate = fulfilledDate;
        public void SetCancelledDate(DateTime? cancelledDate) => CancelledDate = cancelledDate;
        public void SetUser(User user) => User = user;

        public void SetOrderItems(IEnumerable<OrderItem> orderItems)
        {
            _orderItems.Clear();
            _orderItems.AddRange(orderItems);
        }
    }
}
