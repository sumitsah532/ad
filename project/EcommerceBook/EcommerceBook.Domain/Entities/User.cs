using System;
using System.Collections.Generic;

namespace EcommerceBook.Domain.Entities
{
    public enum UserRole
    {
        Member,
        Staff,
        Admin
    }

    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string? PasswordHash { get; set; }
        public UserRole Role { get; private set; }
        public int SuccessfulOrders { get; private set; } = 0;
        public string FullName { get; private set; }
        public string ProfileImageUrl { get; private set; } // Stores the path/URL to the image

        public ICollection<Cart> Carts { get; set; } // Allow multiple carts

        // Navigation properties
        public ICollection<Order> Orders { get; private set; } = new List<Order>();
        public ICollection<Bookmark> Bookmarks { get; private set; } = new List<Bookmark>();
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();

        public void increaseSuccessfulOrder()
        {
            this.SuccessfulOrders++;   
            
        }
        // Constructor
        public User(string email, string passwordHash, UserRole role = UserRole.Member,
                   string fullName = null, string profileImageUrl = null)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash is required.");

            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            FullName = fullName;
            ProfileImageUrl = profileImageUrl;
        }

        // Domain Methods
        public void IncrementSuccessfulOrders() => SuccessfulOrders++;

        public void ChangeRole(UserRole newRole) => Role = newRole;

        public bool IsEligibleForLoyaltyDiscount() => SuccessfulOrders >= 10;

        public void UpdateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be empty.");
            FullName = fullName;
        }

        public void SetPasswordHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Password hash cannot be empty.");
            PasswordHash = hash;
        }

        public void UpdateProfileImage(string imageUrl)
        {
            // You might want to add validation for the URL format
            ProfileImageUrl = imageUrl;
        }

        public string GetRoleAsString() => Role.ToString();
        public string SetandGetImageUrl(string imageurl)
        {

        ProfileImageUrl = imageurl; 
            return ProfileImageUrl;
        
        }
    }
}