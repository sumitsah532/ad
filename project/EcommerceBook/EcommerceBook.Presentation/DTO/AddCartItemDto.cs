namespace EcommerceBook.Presentation.DTO
{
    public class AddCartItemDto
    {
        public Guid? BookId { get; set; }
        public int ?Quantity { get; set; }
        public decimal ?Price { get; set; }
    }

    public class UpdateCartItemQuantityDto
    {
        public int ?NewQuantity { get; set; }
    }
}