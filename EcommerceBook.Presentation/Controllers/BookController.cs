using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using EcommerceBook.Presentation.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBook.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, IConfiguration configuration, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Book>> CreateBook([FromForm] BookCreateDto bookDto)
        {
            if (bookDto == null)
                return BadRequest("Book details are required.");

            if (string.IsNullOrWhiteSpace(bookDto.Title)) return BadRequest("Book title is required.");
            if (string.IsNullOrWhiteSpace(bookDto.Author)) return BadRequest("Author is required.");
            if (string.IsNullOrWhiteSpace(bookDto.ISBN)) return BadRequest("ISBN is required.");
            if (bookDto.Price < 1) return BadRequest("Price must be at least 1.");
            if (bookDto.StockQuantity < 0) return BadRequest("Quantity cannot be negative.");

            string bookImageUrl = null;

            if (bookDto.BookImage != null && bookDto.BookImage.Length > 0)
            {
                try
                {
                    var allowedExtensions = _configuration.GetSection("FileStorageSettings:AllowedExtensions").Get<string[]>();
                    var maxFileSize = _configuration.GetValue<long>("FileStorageSettings:MaxFileSizeMB") * 1024 * 1024;
                    var extension = Path.GetExtension(bookDto.BookImage.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                        return BadRequest($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");

                    if (bookDto.BookImage.Length > maxFileSize)
                        return BadRequest($"File size exceeds the limit of {maxFileSize / (1024 * 1024)}MB");

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["FileStorageSettings:BookImagePath"]);
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await bookDto.BookImage.CopyToAsync(stream);

                    bookImageUrl = $"{_configuration["FileStorageSettings:BookImageBaseUrl"]}/{fileName}";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading book image");
                }
            }

            try
            {
                var newBook = new Book(
                    title: bookDto.Title,
                    author: bookDto.Author,
                    isbn: bookDto.ISBN,
                    price: bookDto.Price,
                    stockQuantity: bookDto.StockQuantity,
                    category: bookDto.Category,
                    bookImageUrl: bookImageUrl,
                    tags: bookDto.Tags
                );

                newBook.SetTags(bookDto.Tags);

                var createdBook = await _bookService.AddBook(newBook);

                return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return StatusCode(500, "An error occurred while creating the book.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBook(Guid id, [FromForm] BookCreateDto bookDto)
        {
            if (bookDto == null)
                return BadRequest("Book details are required.");

            if (string.IsNullOrWhiteSpace(bookDto.Title)) return BadRequest("Book title is required.");
            if (string.IsNullOrWhiteSpace(bookDto.Author)) return BadRequest("Author is required.");
            if (string.IsNullOrWhiteSpace(bookDto.ISBN)) return BadRequest("ISBN is required.");
            if (bookDto.Price < 1) return BadRequest("Price must be at least 1.");
            if (bookDto.StockQuantity < 0) return BadRequest("Quantity cannot be negative.");

            var existingBook = await _bookService.GetBookByIdAsNoTracking(id);
            if (existingBook == null) return NotFound("Book not found.");

            string bookImageUrl = existingBook.BookImageUrl;

            if (bookDto.BookImage != null && bookDto.BookImage.Length > 0)
            {
                try
                {
                    var allowedExtensions = _configuration.GetSection("FileStorageSettings:AllowedExtensions").Get<string[]>();
                    var maxFileSize = _configuration.GetValue<long>("FileStorageSettings:MaxFileSizeMB") * 1024 * 1024;
                    var extension = Path.GetExtension(bookDto.BookImage.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                        return BadRequest($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");

                    if (bookDto.BookImage.Length > maxFileSize)
                        return BadRequest($"File size exceeds the limit of {maxFileSize / (1024 * 1024)}MB");

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["FileStorageSettings:BookImagePath"]);
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    if (!string.IsNullOrEmpty(existingBook.BookImageUrl))
                    {
                        var oldFileName = existingBook.BookImageUrl.Split('/').Last();
                        var oldFilePath = Path.Combine(uploadPath, oldFileName);
                        if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
                    }

                    var fileName = $"{id}{extension}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await bookDto.BookImage.CopyToAsync(stream);

                    bookImageUrl = $"{_configuration["FileStorageSettings:BookImageBaseUrl"]}/{fileName}";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating book image");
                }
            }
            else if (!string.IsNullOrEmpty(bookDto.BookImageUrl))
            {
                bookImageUrl = bookDto.BookImageUrl;
            }

            var updatedBook = new Book(
                title: bookDto.Title,
                author: bookDto.Author,
                isbn: bookDto.ISBN,
                price: bookDto.Price,
                stockQuantity: bookDto.StockQuantity,
                category: bookDto.Category,
                bookImageUrl: bookImageUrl,
                tags: bookDto.Tags ?? existingBook.Tags
            );

            updatedBook.SetGuid(id);

            if (bookDto.IsOnSale && bookDto.SalePrice.HasValue && bookDto.SaleStartDate.HasValue && bookDto.SaleEndDate.HasValue)
            {
                try
                {
                    updatedBook.SetSale(bookDto.SalePrice.Value, bookDto.SaleStartDate.Value, bookDto.SaleEndDate.Value);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Sale information invalid: {ex.Message}");
                }
            }
            else
            {
                updatedBook.EndSale();
            }

            if (bookDto.Tags != null)
                updatedBook.SetTags(bookDto.Tags);

            try
            {
                var result = await _bookService.UpdateBookDetached(updatedBook);
                if (result == null) return StatusCode(500, "Failed to update book.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book");
                return StatusCode(500, "An error occurred while updating the book.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("Invalid book ID format.");

            var result = await _bookService.DeleteBook(guid);
            if (result) return NoContent();
            return NotFound("Book not found.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
                return Ok(books);
        }
    }
}
