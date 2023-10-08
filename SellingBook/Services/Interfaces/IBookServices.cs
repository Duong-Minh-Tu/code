using SellingBook.Dtos.BooksDto;
using SellingBook.Dtos.BooksDto;
using SellingBook.Dtos.ReviewDto;
using SellingBook.Entities;
using SellingBook.Page;

namespace SellingBook.Services.Interfaces
{
    public interface IBookServices
    {
        //BookDto CreateBook(FileUpLoad fileObj, BookDto input);
        List<String> FuzzySreach(string query);
        Task AutoCreateBook();
        BookDto FindById(int id);
        int Deleted(int id);
        void UpdateBook(CreateBookDto input, int id);
        PageResultDto<List<BookDto>> FindAll(FilterBookDto input);
        PageResultDto<BookDailySuggestDto> FindAllDailySuggest(FilterBookDto input);
        PageResultDto<List<BookDto>> FindAllBookUser(FilterBookDto input);
        void Like(Like input);
        void Reviews(Review input, int DetailId);
        void ListImageBook(int IdBook, List<string> iamge);
        /// <summary>
        /// tìm đánh giá của quyển sách
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        List<ReviewsDto> FindAllReview(int idBook);
        List<Discount> FindAllDiscount();
    }
}
