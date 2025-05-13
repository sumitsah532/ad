using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(Guid id);

        // New methods for detached entity updates
        Task<Book> GetByIdAsNoTrackingAsync(Guid id);
        Task<Book> UpdateDetachedAsync(Book book);
    }
}