using SellingBook.Entities;

namespace SellingBook.Dtos.ReviewDto
{
    public class ReviewsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public int BookId { get; set; }
        public string Text { get; set; }
        public int? Star { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public string ImageUser { get; set; }
    }
}
