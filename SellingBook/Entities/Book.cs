namespace SellingBook.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public int DiscountId { get; set; }
        // ten quyen sach
        public string BookName { get; set; }
        // tac gia
        public string Author { get; set; }
        // the loai sach
        public string TypeOfBook { get; set; }
        // gia quyen sach
        public float Price { get; set; }
        public string Image { get; set; }
        // tổng lượt thích
        public int? TotalLike { get; set; }
        // tổng đã bán
        public int? TotalSales { get; set; }
        public float? DiscountPercent { get; set; }
        // tổng số lượt đánh giá
        public float? TotalStar { get; set; }
        // mô tả
        public string Describe { get; set; }
    }
}
