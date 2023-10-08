using SellingBook.Dtos.BooksDto;
using SellingBook.Entities;

namespace SellingBook.Dtos.CartsDto
{
    public class CartDto
    {
        public int IdUser { get; set; }
        public List<Book> IdBook { get; set; }
    }
}
