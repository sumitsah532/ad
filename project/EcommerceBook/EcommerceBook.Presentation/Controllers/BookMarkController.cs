using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Presentation.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EcommerceBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBookmark([FromForm] BookmarkDto dto)
        {
            if (dto.UserId == Guid.Empty || dto.BookId == Guid.Empty || dto.BookId == null)
            {
                return BadRequest("UserId and BookId are required.");
            }

            try
            {
                //var existingBookmark = await CheckExists(dto.UserId.Value, dto.BookId.Value);
                bool existingBookmark = await _bookmarkService.BookmarkExistsAsync(dto.UserId.Value, dto.BookId.Value);

                if (existingBookmark)
                {
                    return Conflict("Bookmark already exists.");
                }

                var bookmark = new Bookmark(dto.UserId.Value, dto.BookId.Value);
                var result = await _bookmarkService.AddBookmarkAsync(bookmark);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while adding the bookmark: {ex.Message}");
            }
        }

        // DELETE: api/Bookmark?userId=xxx&bookId=yyy
        [HttpDelete]
        public async Task<IActionResult> DeleteBookmark([FromQuery] Guid userId, [FromQuery] Guid bookId)
        {
            try
            {

                await _bookmarkService.DeleteBookmarkAsync(userId, bookId);
                return Ok("Bookmark deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("by-id/{bookmarkId}")]
        public async Task<IActionResult> DeleteBookmark(string bookmarkId)
        {
            try
            {

                await _bookmarkService.DeleteBookMarkById(Guid.Parse(bookmarkId));
                return Ok("Bookmark deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound(ex.Message);
            }
        }


        // GET: api/Bookmark/user/123
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookmarksByUser(Guid userId)
        {
            var bookmarks = await _bookmarkService.GetBookmarksByUserIdAsync(userId);
            return Ok(bookmarks);
        }

        // GET: api/Bookmark/exists?userId=xxx&bookId=yyy
        [HttpGet("exists")]
        public async Task<IActionResult> CheckExists([FromQuery] Guid userId, [FromQuery] Guid bookId)
        {
            bool exists = await _bookmarkService.BookmarkExistsAsync(userId, bookId);
            return Ok(new { exists });
        }



    /*    [HttpGet("exists")]
        public async Task<IActionResult> DeleteById([FromQuery] Guid bookMarkId)
        {

            bool exists = await _bookmarkService.BookmarkExistsAsync(userId, bookId);
            return Ok(new { exists });
        }*/
    }
}
