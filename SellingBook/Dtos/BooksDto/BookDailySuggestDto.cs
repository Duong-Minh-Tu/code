namespace SellingBook.Dtos.BooksDto
{
    public class BookDailySuggestDto
    {
        public List<BookDto> LatestProducts { get; set; }
        public List<BookDto> PopularProducts { get; set; }
        public List<BookDto> SellingProducts { get; set; }
        public List<BookDto> TopRatedProducts { get; set; }
    }
}
