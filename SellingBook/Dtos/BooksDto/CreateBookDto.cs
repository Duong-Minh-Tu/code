namespace SellingBook.Dtos.BooksDto
{
    public class CreateBookDto
    {
        // ten quyen sach
        public string BookName { get; set; }
        // tac gia
        public string Author { get; set; }
        // the loai sach
        public string TypeOfBook { get; set; }
        // gia quyen sach
        public int Price { get; set; }
        public string Image { get; set; }
        public float? DiscountPercent { get; set; }
        public string Describe { get; set; }
    }
}
