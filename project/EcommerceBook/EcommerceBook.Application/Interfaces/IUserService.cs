using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Application.IServices
{
    public interface IUserService
    {
        Task<User> AddUser(User user);  // Add a new user
        Task<User> UpdateUser(User user);  // Update an existing user
        Task<bool> DeleteUser(string id);  // Delete a user by their ID (returns success/failure)
        Task<IEnumerable<User>> GetAllUsers();  // Get all users
        Task<User> GetUserById(string id);  // Get a user by their ID
    }
}
