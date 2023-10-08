namespace SellingBook.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public float Total { get; set; }
        public List<CartDetail> cartDetails { get; set; }
    }
}
