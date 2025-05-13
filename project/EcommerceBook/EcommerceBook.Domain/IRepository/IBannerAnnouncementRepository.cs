using EcommerceBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Domain.Repositories
{
    public interface IBannerAnnouncementRepository
    {
        Task<BannerAnnouncement> AddAsync(BannerAnnouncement banner);
        Task<BannerAnnouncement> UpdateAsync(BannerAnnouncement banner);
        Task<BannerAnnouncement> GetByIdAsync(Guid bannerId);
        Task<List<BannerAnnouncement>> GetActiveBannersAsync();
        Task DeleteAsync(BannerAnnouncement banner);
    }
}
