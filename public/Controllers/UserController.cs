using EcommerceBook.Application.IServices;
using EcommerceBook.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using EcommerceBook.Presentation.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Linq;
using System.Net.Mail;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceBook.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IConfiguration configuration,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users.Select(u => new {
                u.Id,
                u.Email,
                u.FullName,
                u.Role,
                u.ProfileImageUrl,
                u.SuccessfulOrders
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Role,
                user.ProfileImageUrl,
                user.SuccessfulOrders
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Login data is required.");
            }

            if (string.IsNullOrWhiteSpace(loginDto.Email))
            {
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Password is required.");
            }

            try
            {
                new MailAddress(loginDto.Email); // Validate email format
            }
            catch
            {
                return BadRequest("Invalid email format.");
            }

            var user = (await _userService.GetAllUsers())
                .FirstOrDefault(u => u.Email.Equals(loginDto.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate JWT token
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpireMinutes")),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            // Return token + user details
            return Ok(new
            {
                Token = jwtToken,
                User = new
                {
                    user.Id,
                    user.Email,
                    user.FullName,
                    Role = user.Role.ToString(),
                    user.ProfileImageUrl
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult<object>> AddUser([FromForm] UserCreateDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(userDto.Email))
            {
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Password is required.");
            }

            if (userDto.Password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters.");
            }

            try
            {
                new MailAddress(userDto.Email);
            }
            catch
            {
                return BadRequest("Invalid email format.");
            }

            // Check for existing user
            if ((await _userService.GetAllUsers())
                .Any(u => u.Email.Equals(userDto.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict("A user with this email already exists.");
            }

            string profileImageUrl = null;

            // Handle image upload
            if (userDto.ProfileImage != null && userDto.ProfileImage.Length > 0)
            {
                var allowedExtensions = _configuration.GetSection("FileStorageSettings:AllowedExtensions").Get<string[]>();
                var maxFileSize = _configuration.GetValue<long>("FileStorageSettings:MaxFileSizeMB") * 1024 * 1024;

                var extension = Path.GetExtension(userDto.ProfileImage.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");
                }

                if (userDto.ProfileImage.Length > maxFileSize)
                {
                    return BadRequest($"File size exceeds the limit of {maxFileSize / (1024 * 1024)}MB");
                }

                try
                {
                    var uploadPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        _configuration["FileStorageSettings:ProfileImagePath"]);

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await userDto.ProfileImage.CopyToAsync(stream);
                    }

                    profileImageUrl = $"{_configuration["FileStorageSettings:ProfileImageBaseUrl"]}/{fileName}";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading profile image");
                    // Continue without image rather than failing registration
                }
            }

            // Hash the password
            var tempUser = new User(userDto.Email, "temp");
            var hashedPassword = _passwordHasher.HashPassword(tempUser, userDto.Password);

            // Create new user
            var newUser = new User(
                email: userDto.Email,
                passwordHash: hashedPassword,
                role: userDto.Role ?? UserRole.Member,
                fullName: userDto.FullName,
                profileImageUrl: profileImageUrl
            );

            var createdUser = await _userService.AddUser(newUser);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, new
            {
                createdUser.Id,
                createdUser.Email,
                createdUser.FullName,
                Role = createdUser.Role.ToString(),
                createdUser.ProfileImageUrl
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] UserUpdateDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Update allowed fields
            existingUser.UpdateFullName(userDto.FullName);
            existingUser.SetandGetImageUrl(userDto.ProfileImageUrl);
           // existingUser.ProfileImageUrl = userDto.ProfileImageUrl;

            if (!string.IsNullOrWhiteSpace(userDto.NewPassword))
            {
                var tempUser = new User(existingUser.Email, "temp");
                existingUser.PasswordHash = _passwordHasher.HashPassword(tempUser, userDto.NewPassword);
            }

            var updatedUser = await _userService.UpdateUser(existingUser);
            return Ok(new
            {
                updatedUser.Id,
                updatedUser.Email,
                updatedUser.FullName,
                Role = updatedUser.Role.ToString(),
                updatedUser.ProfileImageUrl
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var result = await _userService.DeleteUser(id);
                if (!result)
                {
                    return StatusCode(500, new { message = "Failed to delete user" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, new { message = "An error occurred while deleting the user" });
            }
        }
    }
}