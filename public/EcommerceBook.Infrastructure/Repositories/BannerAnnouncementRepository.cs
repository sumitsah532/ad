using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using EcommerceBook.Infrastructure.Persistence; // Assuming you use EF Core
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Infrastructure.Repositories
{
    public class BannerAnnouncementRepository : IBannerAnnouncementRepository
    {
        private readonly BookStoreDbContext _context;

        public BannerAnnouncementRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<BannerAnnouncement> AddAsync(BannerAnnouncement banner)
        {
            await _context.BannerAnnouncements.AddAsync(banner);
            await _context.SaveChangesAsync();
            return banner;  
        }

        public async Task<BannerAnnouncement> UpdateAsync(BannerAnnouncement banner)
        {
            _context.BannerAnnouncements.Update(banner);
            await _context.SaveChangesAsync();
            return banner;
        }

        public async Task<BannerAnnouncement> GetByIdAsync(Guid bannerId)
        {
            return await _context.BannerAnnouncements.FindAsync(bannerId);
        }

        public async Task<List<BannerAnnouncement>> GetActiveBannersAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.BannerAnnouncements
                                 .Where(b => b.IsActive && b.StartTime <= now && b.EndTime >= now)
                                 .ToListAsync();
        }

        public async Task DeleteAsync(BannerAnnouncement banner)
        {
            _context.BannerAnnouncements.Remove(banner);
            await _context.SaveChangesAsync();
        }
    }
}
