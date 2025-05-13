using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BookStoreDbContext _context;

        public CartRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        // Get the active cart for a user
        public async Task<Cart> GetCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Get a cart by its ID
        public async Task<Cart> GetCartByIdAsync(Guid cartId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        // Add a new cart (or reactivate an existing one)
        public async Task<Cart> AddCartAsync(Cart cart)
        {
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId && c.IsActive);

            if (existingCart != null) 
            {
                // Reactivate the existing cart if it exists
                existingCart.UpdateCartStatus(true); // Set IsActive to true
                _context.Carts.Update(existingCart);
                await _context.SaveChangesAsync();
                return existingCart;
            }

            // No active cart found, create a new one
            await _context.Carts.AddAsync(cart);
            try
            {

                await _context.SaveChangesAsync();
                return cart;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return cart;
            }
        }


        // Update the details of an existing cart
        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        // Delete a cart by its ID
        public async Task<bool> DeleteCartAsync(Guid cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null) return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get a cart item by its ID
        public async Task<CartItem> GetCartItemByIdAsync(Guid cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Book)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }

        // Add a new cart item (check for duplicates first)
        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartItem.CartId && ci.BookId == cartItem.BookId);

            if (existingItem != null)
            {
                // Optionally, update quantity or return existing item
                return existingItem;
            }

            // Add the new cart item
            await _context.CartItems.AddAsync(cartItem);
            try
            {

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return cartItem;
        }

        // Update an existing cart item
        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        // Delete a cart item by its ID
        public async Task<bool> DeleteCartItemAsync(Guid cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        // Clear all items from the cart
        public async Task<bool> ClearCartItemsAsync(Guid cartId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cartItems = await _context.CartItems
                    .Where(ci => ci.CartId == cartId)
                    .ToListAsync();

                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                // Rollback the transaction if something goes wrong
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
