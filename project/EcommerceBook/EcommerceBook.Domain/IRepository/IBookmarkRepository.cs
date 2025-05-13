using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBook.Domain.Entities;

namespace EcommerceBook.Domain.Repositories
{
    public interface IBookmarkRepository
    {
        Task AddAsync(Bookmark bookmark);
        Task<bool> ExistsAsync(Guid bookMarkId);

        // ✅ New: Exists by UserId and BookId
        Task<bool> ExistsAsync(Guid userId, Guid bookId);

        Task<List<Bookmark>> GetBookmarksByUserIdAsync(Guid userId);
        Task DeleteAsync(Guid bookMarkId);

        // ✅ New: Delete by UserId and BookId
        Task DeleteByUserAndBookAsync(Guid userId, Guid bookId);

        Task DeleteBookMarkById(Guid bookMarkId);

    }
}
