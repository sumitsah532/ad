using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.Application.Services
{
    public class BannerAnnouncementService : IBannerAnnouncementService
    {
        private readonly IBannerAnnouncementRepository _bannerRepository;

        // Use interface rather than concrete class for repository
        public BannerAnnouncementService(IBannerAnnouncementRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<BannerAnnouncement> CreateBannerAsync(string title, string message, DateTime startTime, DateTime endTime)
        {
            var banner = new BannerAnnouncement(title, message, startTime, endTime);
            return await _bannerRepository.AddAsync(banner);
        }

        public async Task<BannerAnnouncement> UpdateBannerAsync(Guid bannerId, string title, string message, DateTime startTime, DateTime endTime)
        {
            var banner = await _bannerRepository.GetByIdAsync(bannerId);
            if (banner == null)
                throw new ArgumentException("Banner not found.");

            banner.UpdateTitle(title);
            banner.UpdateMessage(message);
            banner.UpdateStartTime(startTime);
            banner.UpdateEndTime(endTime);

            return await _bannerRepository.UpdateAsync(banner);
        }

        public async Task<BannerAnnouncement> GetBannerByIdAsync(Guid bannerId)
        {
            return await _bannerRepository.GetByIdAsync(bannerId);
        }

        public async Task<List<BannerAnnouncement>> GetActiveBannersAsync()
        {
            return await _bannerRepository.GetActiveBannersAsync();
        }

        public async Task DeleteBannerAsync(Guid bannerId)
        {
            var banner = await _bannerRepository.GetByIdAsync(bannerId);
            if (banner != null)
            {
                await _bannerRepository.DeleteAsync(banner);
            }
        }
    }
}
