using EcommerceBook.Application.IServices;
using EcommerceBook.Application.Services;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Interfaces;
using EcommerceBook.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommerceBook.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;

        public OrdersController(
             IEmailSender emailSender,
            IOrderService orderService,
            ILogger<OrdersController> logger,
        IUserService userService)
        {
            _orderService = orderService;
            _logger = logger;
            _emailSender = emailSender;
            _userService = userService;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(new OrderDto(order));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Order not found");
                return NotFound(new ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input");
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
        {
            var userId = GetCurrentUserId();
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders.Select(o => new OrderDto(o)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userId = GetCurrentUserId();

            if (request?.Items == null || !request.Items.Any())
            {
                _logger.LogWarning("Empty order request received by user {UserId}", userId);
                return BadRequest(new ErrorResponse("Order must contain at least one item."));
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(
                    userId,
                    request.Items.Select(i => new CartItem(i.BookId, i.Quantity)));

                _logger.LogInformation("Order created: {OrderId} for user {UserId}", order.Id, userId);

                // Send invoice email
                var userEmail = await _userService.GetUserById(userId.ToString()); // Implement this if not done
             string emailBody = $@"
<!DOCTYPE html>
<html>
<head>
  <style>
    body {{
      font-family: Arial, sans-serif;
      color: #333;
    }}
    .invoice-box {{
      max-width: 800px;
      margin: auto;
      padding: 20px;
      border: 1px solid #eee;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
      line-height: 24px;
    }}
    .heading {{
      background: #f4f4f4;
      border-bottom: 1px solid #ddd;
      font-weight: bold;
    }}
    table {{
      width: 100%;
      line-height: inherit;
      text-align: left;
      border-collapse: collapse;
    }}
    table td, table th {{
      padding: 8px;
      border: 1px solid #ddd;
    }}
    .total {{
      font-weight: bold;
    }}
    .text-right {{
      text-align: right;
    }}
    .title {{
      font-size: 26px;
      font-weight: bold;
      color: #4CAF50;
    }}
  </style>
</head>
<body>
  <div class='invoice-box'>
    <div class='title'>Invoice</div>
    <p><strong>Order ID:</strong> {order.Id}<br />
    <strong>Claim Code:</strong> {order.ClaimCode}<br />
    <strong>Order Date:</strong> {order.OrderDate:yyyy-MM-dd HH:mm}<br />
    <strong>PAN No:</strong> 123456789 (Demo)</p>

    <table>
      <thead>
        <tr class='heading'>
          <th>Book Title</th>
          <th>Quantity</th>
          <th>Unit Price</th>
          <th class='text-right'>Subtotal</th>
        </tr>
      </thead>
      <tbody>
        {string.Join("", order.OrderItems.Select(i => $@"
          <tr>
            <td>{i.BookTitle}</td>
            <td>{i.Quantity}</td>
            <td>${i.Price:F2}</td>
            <td class='text-right'>${(i.Price * i.Quantity):F2}</td>
          </tr>
        "))}
        <tr>
          <td colspan='3' class='text-right total'>Total Before Discount</td>
          <td class='text-right total'>${order.OrderItems.Sum(i => i.Price * i.Quantity):F2}</td>
        </tr>
        <tr>
          <td colspan='3' class='text-right'>Discount</td>
          <td class='text-right'>- ${order.DiscountTotal:F2}</td>
        </tr>
        <tr>
          <td colspan='3' class='text-right total'>Total Amount</td>
          <td class='text-right total'>${order.TotalPrice:F2}</td>
        </tr>
      </tbody>
    </table>

    <p>Thank you for shopping with us!</p>
  </div>
</body>
</html>";


                await _emailSender.SendEmailAsync(userEmail.Email, $"Invoice - Order #{order.Id}", emailBody);

                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { id = order.Id },
                    new OrderDto(order));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation failed while creating order for user {UserId}", userId);
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Missing cart item while creating order for user {UserId}", userId);
                return NotFound(new ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating order for user {UserId}", userId);
                return StatusCode(500, new ErrorResponse("An unexpected error occurred."));
            }
        }

        /*  public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
          {
              var userId = GetCurrentUserId();

              if (request?.Items == null || !request.Items.Any())
              {
                  _logger.LogWarning("Empty order request received by user {UserId}", userId);
                  return BadRequest(new ErrorResponse("Order must contain at least one item."));
              }

              try
              {
                  var order = await _orderService.CreateOrderAsync(
                      userId,
                      request.Items.Select(i => new CartItem(i.BookId, i.Quantity)));

                  _logger.LogInformation("Order created: {OrderId} for user {UserId}", order.Id, userId);

                  return CreatedAtAction(
                      nameof(GetOrderById),
                      new { id = order.Id },
                      new OrderDto(order));
              }
              catch (ArgumentException ex)
              {
                  _logger.LogWarning(ex, "Validation failed while creating order for user {UserId}", userId);
                  return BadRequest(new ErrorResponse(ex.Message));
              }
              catch (KeyNotFoundException ex)
              {
                  _logger.LogWarning(ex, "Missing cart item while creating order for user {UserId}", userId);
                  return NotFound(new ErrorResponse(ex.Message));
              }
              catch (Exception ex)
              {
                  _logger.LogError(ex, "Unexpected error while creating order for user {UserId}", userId);
                  return StatusCode(500, new ErrorResponse("An unexpected error occurred."));
              }
          }*/

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            try
            {
                var success = await _orderService.CancelOrderAsync(id);
                if (!success) return NotFound();

                _logger.LogInformation("Order cancelled: {OrderId}", id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Cannot cancel order");
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpPut("fulfill/{claimCode}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> FulfillOrder(string claimCode)
        {
            try
            {
                var success = await _orderService.FulfillOrderAsync(claimCode);
                if (!success) return NotFound();

                _logger.LogInformation("Order fulfilled: {ClaimCode}", claimCode);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid claim code");
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders.Select(o => new OrderDto(o)));
        }

        [HttpGet("order-books")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrderBooks()
        {
            var userId = GetCurrentUserId();


            var books = await _orderService.GetAllOrderBooks(userId);
            return Ok(books);
/*            return Ok(orders.Select(o => new OrderDto(o)));
*/        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders.Select(o => new OrderDto(o)));
        }

        [HttpGet("recent/{days}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetRecentOrders(int days)
        {
            try
            {
                var userId = GetCurrentUserId();
                var orders = await _orderService.GetRecentOrdersByUserAsync(userId, days);
                return Ok(orders.Select(o => new OrderDto(o)));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid days parameter");
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in claims");

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Invalid user ID format in token");

            return userId;
        }

        // DTO classes
        public record CreateOrderRequest(
            [Required][MinLength(1)] List<OrderItemRequest> Items);

        public record OrderItemRequest(
            [Required] Guid BookId,
            [Required][Range(1, int.MaxValue)] int Quantity);

        public record OrderDto(
     Guid Id,
     DateTime OrderDate,
     decimal TotalPrice,
     OrderStatus Status,
     string ClaimCode,
     decimal DiscountTotal,
     bool IsFulfilled,
     IEnumerable<OrderItemDto> Items)
        {
            public OrderDto(Order order) : this(
                order.Id,
                order.OrderDate,
                order.TotalPrice,
                order.Status,
                order.ClaimCode,
                order.DiscountTotal,
                order.IsFulfilled,
                order.OrderItems.Select(i => new OrderItemDto(i)))
            {
            }
        }


        public record OrderItemDto(
            Guid BookId,
            string BookTitle,
            int Quantity,
            decimal Price)
        {
            public OrderItemDto(OrderItem item) : this(
                item.BookId,
                item.BookTitle,
                item.Quantity,
                item.Price)
            {
            }
        }

        public record ErrorResponse(string Message);
    }
}