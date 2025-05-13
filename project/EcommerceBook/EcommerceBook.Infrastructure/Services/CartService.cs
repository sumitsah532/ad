using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace EcommerceBook.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;

        public CartService(ICartRepository cartRepository, IBookRepository bookRepository)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Cart> GetUserCartAsync(Guid userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<Cart> AddItemToCartAsync(Guid userId, Guid bookId, int quantity, decimal price)
        {
            // Validate book exists
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new ArgumentException("Book not found");

            // Get or create cart
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart(userId);
                await _cartRepository.AddCartAsync(cart);
            }

            // Create cart item
            var cartItem = new CartItem(cart.Id, bookId, quantity, price);
           
           

            await _cartRepository.AddCartItemAsync(cartItem);
            return await _cartRepository.GetCartByIdAsync(cart.Id);
        }

        public async Task<bool> RemoveItemFromCartAsync(Guid userId, Guid cartItemId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null || cartItem.CartId != cart.Id) return false;

            return await _cartRepository.DeleteCartItemAsync(cartItemId);
        }

        public async Task<bool> UpdateCartItemQuantityAsync(Guid userId, Guid cartItemId, int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null || cartItem.CartId != cart.Id) return false;
            cartItem.UpdateQuantity(newQuantity);

          /*  cartItem.se

            cartItem.Quantity = newQuantity;*/
            await _cartRepository.UpdateCartItemAsync(cartItem);
            return true;
        }

        public async Task<bool> ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            return await _cartRepository.ClearCartItemsAsync(cart.Id);
        }
    }
}