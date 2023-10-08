namespace SellingBook.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float? DiscountPercent { get; set; }
        public string Active { get; set; }
    }
}
