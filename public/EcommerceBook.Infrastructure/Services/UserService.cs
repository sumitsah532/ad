using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUser(User user)
        {
            // Example of validation: Check if the email already exists
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("A user with this email already exists.");
            }

            return await _userRepository.AddAsync(user);
        }

        public async Task<User> UpdateUser(User user)
        {
            // Example of validation: Check if the user exists
            var existingUser = await _userRepository.GetByIdAsync( user.Id.ToString());
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUser(string id)
        {
            // Validate if the user exists before deleting
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            return user;
        }
    }
}
