using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);  // Typically, delete returns a boolean or status code.
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);  // This is for fetching a single user by their ID.
        Task<User> GetByEmailAsync(string email);  // If you plan to check if an email exists.
    }
}
