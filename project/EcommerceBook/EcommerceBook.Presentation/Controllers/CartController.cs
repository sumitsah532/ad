using EcommerceBook.Application.IServices;
using EcommerceBook.Presentation.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EcommerceBook.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCart()
        {
            var userId = GetCurrentUserId();
            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDto cartItemDto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var cart = await _cartService.AddItemToCartAsync(
                    userId,
                    cartItemDto.BookId?? new Guid(),
                    cartItemDto.Quantity??1,
                    cartItemDto.Price??1);

                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveItemFromCart(Guid cartItemId)
        {
            var userId = GetCurrentUserId();
            var result = await _cartService.RemoveItemFromCartAsync(userId, cartItemId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(
            Guid cartItemId,
            [FromBody] UpdateCartItemQuantityDto updateDto)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _cartService.UpdateCartItemQuantityAsync(
                    userId, cartItemId, updateDto.NewQuantity??1);

                if (!result) return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            var result = await _cartService.ClearCartAsync(userId);
            if (!result) return NotFound();
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in claims");

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Invalid user ID format in token");

            return userId;
        }

    }
}