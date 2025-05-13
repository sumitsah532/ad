using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreDbContext _context;

        public UserRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(Guid.Parse(id));
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            try
            {

                var user = await _context.Users.FindAsync(Guid.Parse(id));
                return user;
            }
            catch (Exception ex) {
                return null;
              }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
