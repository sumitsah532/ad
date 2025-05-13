using System;

namespace EcommerceBook.Domain.Entities
{
    public class BannerAnnouncement
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool IsActive { get; private set; }

        // For EF Core
        private BannerAnnouncement() { }

        // Constructor for initializing a new banner announcement
        public BannerAnnouncement(string title, string message, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.");
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be empty.");
            if (endTime <= startTime) throw new ArgumentException("End time must be after start time.");

            Id = Guid.NewGuid();
            Title = title;
            Message = message;
            StartTime = startTime;
            EndTime = endTime;
            IsActive = true; // Initially active when created
        }

        // Getter and Setter Methods for Updating Values
        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.");
            Title = title;
        }

        public void UpdateMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be empty.");
            Message = message;
        }

        public void UpdateStartTime(DateTime startTime)
        {
            if (startTime >= EndTime) throw new ArgumentException("Start time must be before end time.");
            StartTime = startTime;
        }

        public void UpdateEndTime(DateTime endTime)
        {
            if (endTime <= StartTime) throw new ArgumentException("End time must be after start time.");
            EndTime = endTime;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        // Method to check if the banner is currently active (between start and end times)
        public bool IsCurrentlyActive()
        {
            var now = DateTime.UtcNow;
            return IsActive && StartTime <= now && EndTime >= now;
        }
    }
}
