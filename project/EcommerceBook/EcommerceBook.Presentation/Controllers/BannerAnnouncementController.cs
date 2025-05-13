using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerAnnouncementController : ControllerBase
    {
        private readonly IBannerAnnouncementService _bannerAnnouncementService;

        public BannerAnnouncementController(IBannerAnnouncementService bannerAnnouncementService)
        {
            _bannerAnnouncementService = bannerAnnouncementService;
        }

        // GET: api/BannerAnnouncement/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBannerById(Guid id)
        {
            var banner = await _bannerAnnouncementService.GetBannerByIdAsync(id);
            if (banner == null)
                return NotFound("Banner not found.");

            return Ok(banner);
        }

        // GET: api/BannerAnnouncement/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveBanners()
        {
            var activeBanners = await _bannerAnnouncementService.GetActiveBannersAsync();
            return Ok(activeBanners);
        }

        // POST: api/BannerAnnouncement
        [HttpPost]
        public async Task<IActionResult> CreateBanner([FromBody] BannerAnnouncementRequest request)
        {
            if (request == null)
                return BadRequest("Invalid data.");

            var banner = await _bannerAnnouncementService.CreateBannerAsync(request.Title, request.Message, request.StartTime, request.EndTime);
            return CreatedAtAction(nameof(GetBannerById), new { id = banner.Id }, banner);
        }

        // PUT: api/BannerAnnouncement/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBanner(Guid id, [FromBody] BannerAnnouncementRequest request)
        {
            if (request == null)
                return BadRequest("Invalid data.");

            try
            {
                var updatedBanner = await _bannerAnnouncementService.UpdateBannerAsync(id, request.Title, request.Message, request.StartTime, request.EndTime);
                return Ok(updatedBanner);
            }
            catch (ArgumentException)
            {
                return NotFound("Banner not found.");
            }
        }

        // DELETE: api/BannerAnnouncement/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            await _bannerAnnouncementService.DeleteBannerAsync(id);
            return NoContent();
        }
    }

    // DTO used for POST and PUT requests
    public class BannerAnnouncementRequest
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
