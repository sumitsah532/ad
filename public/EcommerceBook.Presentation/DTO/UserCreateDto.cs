using EcommerceBook.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EcommerceBook.Presentation.DTO
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        public string FullName { get; set; }
        public UserRole? Role { get; set; }

        // For file upload
        public IFormFile ProfileImage { get; set; }
    }

        public class UserUpdateDto
        {
            public string Id { get; set; }                    // Required for identity match during update
            public string FullName { get; set; }              // Optional: to update user's full name
            public string ProfileImageUrl { get; set; }       // Optional: to update profile image URL
            public string NewPassword { get; set; }           // Optional: to update password
        }

}
