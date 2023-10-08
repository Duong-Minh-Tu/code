namespace SellingBook.Entities
{
    public class CartDetail
    {
        public int Id { get; set; }
        public int IdCart { get; set; }
        public int IdBook { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public Book book { get; set; }
    }
}
