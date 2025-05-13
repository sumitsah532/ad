using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository;

        public BookmarkService(IBookmarkRepository bookmarkRepository)
        {
            _bookmarkRepository = bookmarkRepository;
        }

        public async Task<Bookmark> AddBookmarkAsync(Bookmark bookmark)
        {
            bool exists = await _bookmarkRepository.ExistsAsync(bookmark.UserId, bookmark.BookId);
            if (exists)
            {
                throw new InvalidOperationException("Bookmark already exists.");
            }

            await _bookmarkRepository.AddAsync(bookmark);
            return bookmark;
        }

        // ✅ Delete bookmark by userId and bookId
        public async Task<bool> DeleteBookmarkAsync(Guid userId, Guid bookId)
        {
            bool exists = await _bookmarkRepository.ExistsAsync(userId, bookId);
            if (!exists)
            {
                throw new KeyNotFoundException("Bookmark not found.");
            }

            await _bookmarkRepository.DeleteByUserAndBookAsync(userId, bookId);
            return true;
        }

       // Task<bool> DeleteBookMarkById(Guid bookMarkId);

        public async Task<bool> DeleteBookMarkById(Guid bookMarkId)
        {
            bool exists = await _bookmarkRepository.ExistsAsync(bookMarkId);
            if (!exists)
            {
                throw new KeyNotFoundException("Bookmark not found.");
            }

            await _bookmarkRepository.DeleteBookMarkById(bookMarkId);
            return true;
        }

        public async Task<bool> BookmarkExistsAsync(Guid userId, Guid bookId)
        {
            return await _bookmarkRepository.ExistsAsync(userId, bookId);
        }

        public async Task<IEnumerable<Bookmark>> GetBookmarksByUserIdAsync(Guid userId)
        {
            return await _bookmarkRepository.GetBookmarksByUserIdAsync(userId);
        }


    }
}
