namespace SellingBook.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Text { get; set; }
        public int? Star { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
    }
}
