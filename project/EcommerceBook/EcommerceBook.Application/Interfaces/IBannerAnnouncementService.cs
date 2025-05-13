using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Application.IServices
{
    public interface IBannerAnnouncementService
    {
        Task<BannerAnnouncement> CreateBannerAsync(string title, string message, DateTime startTime, DateTime endTime);
        Task<BannerAnnouncement> UpdateBannerAsync(Guid bannerId, string title, string message, DateTime startTime, DateTime endTime);
        Task<BannerAnnouncement> GetBannerByIdAsync(Guid bannerId);
        Task<List<BannerAnnouncement>> GetActiveBannersAsync();
        Task DeleteBannerAsync(Guid bannerId);
    }
}
