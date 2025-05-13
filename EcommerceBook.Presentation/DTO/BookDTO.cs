namespace EcommerceBook.Presentation.DTO
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public IFormFile? BookImage { get; set; }  // For new image uploads
        public string? BookImageUrl { get; set; }  // For existing image URL
        public bool IsOnSale { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? SaleStartDate { get; set; }
        public DateTime? SaleEndDate { get; set; }
        public List<string>? Tags { get; set; }
    }


    public class BookUpdateDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public IFormFile BookImage { get; set; }
        public bool RemoveImage { get; set; }
        public bool IsOnSale { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? SaleStartDate { get; set; }
        public DateTime? SaleEndDate { get; set; }
        public List<string> Tags { get; set; }
    }
        public class BookmarkDto
        {
            public Guid? UserId { get; set; }
            public Guid? BookId { get; set; }
        
    }


}
