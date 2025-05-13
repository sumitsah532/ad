using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Application.IServices
{
    public interface IBookmarkService
    {
        Task<Bookmark> AddBookmarkAsync(Bookmark bookmark);

        // ✅ Updated to match logic
        Task<bool> DeleteBookmarkAsync(Guid userId, Guid bookId);
        Task<bool> DeleteBookMarkById(Guid bookMarkId);


        Task<bool> BookmarkExistsAsync(Guid userId, Guid bookId);

        
    }
}
