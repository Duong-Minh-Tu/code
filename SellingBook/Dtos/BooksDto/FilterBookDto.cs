using SellingBook.Page;

namespace SellingBook.Dtos.BooksDto
{
    public class FilterBookDto : FilterDto
    {
        public int? FiterPrice { get; set; }
        public int? NewBook { get; set; }
        public int? TotalSell { get; set; }
        public string? TypeOfBook { get; set; }
    }
}
