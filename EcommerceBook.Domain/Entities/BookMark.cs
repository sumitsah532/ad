using EcommerceBook.Domain.Entities;

public class Bookmark
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid BookId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public User User { get; private set; }
    public Book Book { get; private set; } // <-- Add this back

    // Constructor
    public Bookmark(Guid userId, Guid bookId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        BookId = bookId;
        CreatedAt = DateTime.UtcNow;
    }
}
