namespace SellingBook.Dtos.Discounts
{
    public class CreateDiscountDto
    {
        public string Name { get; set; }
        public float? DiscountPercent { get; set; }
        public string Active { get; set; }
    }
}
