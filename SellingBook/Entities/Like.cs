namespace SellingBook.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public char IsDeleted { get; set; }
    }
}
