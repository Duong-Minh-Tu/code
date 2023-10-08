using SellingBook.DbContexts;
using SellingBook.Dtos.BooksDto;
using SellingBook.Dtos.ReviewDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;

namespace SellingBook.Services.Implements
{
    public class BookServices : IBookServices
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContext;

        public BookServices(ILogger<BookServices> logger,
            ApplicationDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContext
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContext;
        }

        public CreateBookDto CreateBook(CreateBookDto input)
        {
            var book = _dbContext.books.Add(new Book()
            {
                BookName = input.BookName,
                Author = input.Author,
                TypeOfBook = input.TypeOfBook,
                Price = input.Price,
                DiscountPercent = input.DiscountPercent,
                Image = input.Image,
                Describe = input.Describe,
            });
            _dbContext.SaveChanges();
            return input;
        }

        public async Task AutoCreateBook()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Random random = new Random();

            string filePath = @"D:\APIsellingBook\SellingBook\bookdata.xlsx"; // Đường dẫn đến tệp Excel của bạn
            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                try
                {
                    ExcelWorksheet worksheetx = package.Workbook.Worksheets[0];
                    // Rest of your code
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 để bỏ qua hàng tiêu đề
                {
                    int randomPrice = random.Next(100000, 1000001);
                    int randomDiscountPercent = random.Next(15, 70);
                    int randomNumberOfPage = random.Next(100, 10000);

                    string tenSach = worksheet.Cells[row, 1].Value.ToString(); // Lấy thông tin từ cột 1
                    string tenTacGia = worksheet.Cells[row, 2].Value.ToString(); // Lấy thông tin từ cột 2
                    string ImageUrl = "https://manybooks.net" + worksheet.Cells[row, 3].Value.ToString();
                    var Book = _dbContext.books.Add(new Book()
                    {
                        BookName = tenSach,
                        Author = tenTacGia,
                        TypeOfBook = RandomTypeOfBook(),    // thể loại sách
                        Price = randomPrice,
                        DiscountPercent = randomDiscountPercent,
                        Image = ImageUrl,
                        Describe = RandomDescribe(),
                    });
                }
            }
            _dbContext.SaveChanges();
        }

        public string RandomDescribe()
        {
            List<string> congTySach = new List<string>
            {
                "Cuốn sách này là một tác phẩm vô cùng lôi cuốn, được viết bởi tác giả nổi tiếng. Nó là một câu chuyện lãng mạn đặt trong một thị trấn nhỏ nằm giữa những ngọn đồi xanh tươi, nơi mà mùa xuân luôn là thời điểm đẹp nhất trong năm. Trong cuốn sách này, chúng ta theo chân hai nhân vật chính, Elizabeth và William, người gặp nhau một cách tình cờ và phát triển một mối tình đáng nhớ. Cuốn sách đi sâu vào tâm hồn của họ, khám phá tình yêu và hy vọng trong những khoảnh khắc đáng trân trọng. Bằng lối viết tinh tế và lời kể trôi chảy, tác giả đã tạo nên một tác phẩm đầy cảm xúc và đẹp đẽ về mối tình đầu và những ngày xuân tươi đẹp đã qua.",
                "Cuốn sách này là một cuốn sách phiêu lưu đầy mạo hiểm và kỳ thú. Nhân vật chính của chúng ta bắt đầu cuộc hành trình vào một khu rừng sâu hoang dã, đầy rẫy với những bí ẩn và khám phá. Cuốn sách mô tả cảm giác của người thám hiểm khi đối mặt với thiên nhiên hoang dã, từ những con đường bụi đầy thách thức đến những thảm thực vật kỳ diệu. Trong suốt cuộc hành trình, nhân vật chính phát triển từ một người tò mò thành một người hiểu biết về giá trị của cuộc sống và thiên nhiên. Cuốn sách này là một sự kết hợp tuyệt vời giữa phiêu lưu và sự thấu hiểu về thế giới tự nhiên.",
                "Cuốn sách này là một tài liệu lập trình sáng tạo dành cho người mới bắt đầu và những người muốn tham gia vào thế giới lập trình sáng tạo. Cuốn sách này hướng dẫn độc giả về cách sử dụng ngôn ngữ lập trình Python để tạo ra các dự án độc đáo và sáng tạo. Từ những kiến thức cơ bản như biến số và lệnh điều kiện đến các chủ đề phức tạp như đồ họa máy tính và trò chơi, cuốn sách này giúp độc giả phát triển kỹ năng lập trình và thúc đẩy sự sáng tạo.",
                "Cuốn sách này là một tác phẩm chuyên sâu về nghệ thuật lãnh đạo. Cuốn sách này giới thiệu đến độc giả mười nguyên tắc quan trọng để trở thành một người lãnh đạo hiệu quả. Tác giả dựa vào nhiều năm nghiên cứu và thực tiễn để cung cấp hướng dẫn chi tiết về cách xây dựng mối quan hệ tốt, định hình tầm nhìn và đạt được mục tiêu. Cuốn sách này sẽ là nguồn cảm hứng cho những ai muốn phát triển kỹ năng lãnh đạo và tạo nên sự thành công trong công việc và cuộc sống.",
                "Cuốn sách này là một nguồn tài liệu quý báu cho những người làm việc từ xa hoặc muốn hiểu cách quản lý và làm việc trong môi trường làm việc đa địa điểm. Cuốn sách này cung cấp các chiến lược, công cụ và lời khuyên về cách tạo ra môi trường"
            };

            Random random = new Random();
            int indexNgauNhien = random.Next(congTySach.Count);
            string congTySachNgauNhien = congTySach[indexNgauNhien];
            return congTySachNgauNhien;
        }

        public string RandomTypeOfBook()
        {
            List<string> danhSachTheLoaiSach = new List<string>
            {
                "Tiểu thuyết",
                "Sách phi hư cấu",
                "Tự truyện",
                "Sách dành cho thiếu nhi",
                "Sách khoa học viễn tưởng và fantasy",
                "Sách tự phát triển cá nhân",
                "Sách về lịch sử và văn hóa",
                "Sách kinh doanh và tài chính",
                "Sách tham khảo"
            };

            // Tạo một số ngẫu nhiên để chọn một thể loại sách từ danh sách
            Random random = new Random();
            int indexNgauNhien = random.Next(danhSachTheLoaiSach.Count);
            string theLoaiSachNgauNhien = danhSachTheLoaiSach[indexNgauNhien];
            return theLoaiSachNgauNhien;
        }

        public BookDto FindById(int id)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var bookQuery = _dbContext.books.AsQueryable();
            var bookFind = bookQuery.FirstOrDefault(s => s.Id == id);
            List<Review> review = new List<Review>();
            if (bookFind == null)
            {
                throw new UserFriendlyException("khong tim thay sach");
            }
            else if (bookFind != null)
            {
                var reviewFind = _dbContext.reviews.AsQueryable().Where(o => o.BookId == bookFind.Id).ToList();
                review = reviewFind;
            }
            var bookItem = _mapper.Map<BookDto>(bookFind);
            return bookItem;
        }

        public int Deleted(int id)
        {
            var bookQuery = _dbContext.books.AsQueryable();
            var bookFind = bookQuery.FirstOrDefault(s => s.Id == id);
            if (bookFind == null)
            {
                throw new UserFriendlyException("khong tim thay sach");
            }
            _dbContext.books.Remove(bookFind);
            _dbContext.SaveChanges();
            return 0;
        }

        public void UpdateBook(CreateBookDto input, int id)
        {
            var bookQuery = _dbContext.books.AsQueryable();
            var bookFind = bookQuery.FirstOrDefault(s => s.Id == id);
            if (bookFind == null)
            {
                throw new UserFriendlyException("khong tim thay sach");
            }
            bookFind.BookName = input.BookName;
            bookFind.Author = input.Author;
            bookFind.TypeOfBook = input.TypeOfBook;
            bookFind.Price = input.Price;
            _dbContext.SaveChanges();
        }

        public PageResultDto<List<BookDto>> FindAll(FilterBookDto input)
        {
            var bookQuery = _dbContext.books.AsQueryable();
            if (input.Keyword != null)
            {
                bookQuery = bookQuery.Where(s => s.BookName != null && s.BookName.Contains(input.Keyword));
            }

            var restul = new List<BookDto>();
            var numbers = new List<int?>();
            var countStar = 0;

            foreach (var item in bookQuery)
            {
                var ReviewQuery = _dbContext.reviews.AsQueryable().Where(o => o.Id == item.Id).ToList();
                foreach (var items in ReviewQuery)
                {
                    if (items != null)
                    {
                        int? sum = items.Star;
                        numbers.Add(sum);
                        countStar += 1;
                    }
                }

                var TotalSumm = (from x in numbers select x).Sum();
                float? averageStar = null;
                if (countStar != 0)
                {
                    averageStar = (TotalSumm / countStar);
                }
                else if (countStar == 0)
                {
                    averageStar = null;
                }

                restul.Add(new BookDto()
                {
                    Id = item.Id,
                    BookName = item.BookName,
                    Author = item.Author,
                    TypeOfBook = item.TypeOfBook,
                    Price = item.Price,
                    Image = item.Image,
                    TotalStar = averageStar,
                    DiscountPercent = item.DiscountPercent,
                    TotalLike = item.TotalLike,
                    TotalSales = item.TotalSales,
                    
                    Describe = item.Describe,
                });
            }

            if (input.FiterPrice == null)
            {
                restul = restul;
            }
            else if (input.FiterPrice == 1)
            {
                restul = restul.OrderBy(o => o.Price).ToList();
            }
            else if (input.FiterPrice == 2)
            {
                restul = restul.OrderByDescending(o => o.Price).ToList();
            }

            if (input.NewBook == null)
            {
                restul = restul;
            }
            else if (input.NewBook == 1)
            {
                restul = restul.OrderByDescending(o => o.Id).ToList();
            }

            if (input.TotalSell == null)
            {
                restul = restul;
            }
            else if (input.TotalSell == 1)
            {
                restul = restul.OrderByDescending(o => o.TotalSales).ToList();
            }

            // theo thể loại sách
            if (input.TypeOfBook == null)
            {
                restul = restul;
            }
            else
            {
                restul = restul.OrderByDescending(o => o.TypeOfBook == input.TypeOfBook).ToList();
            }

            if (input.PageSize == -1)
            {
                restul = restul;
            }
            else
            {
                restul = restul.Skip(input.PageSize * (input.PageIndex - 1)).Take(input.PageSize).ToList();
            }
            int totalItem = restul.Count();

            return new PageResultDto<List<BookDto>>
            {
                Item = restul,
                TotalItem = totalItem
            };
        }

        public PageResultDto<BookDailySuggestDto> FindAllDailySuggest(FilterBookDto input)
        {
            var bookQuery = _dbContext.books.AsQueryable();
            if (input.Keyword != null)
            {
                bookQuery = bookQuery.Where(s => s.BookName != null && s.BookName.Contains(input.Keyword));
            }

            var restul = new BookDailySuggestDto();
            var restuls = new List<BookDto>();

            var numbers = new List<int?>();
            var countStar = 0;

            foreach (var item in bookQuery)
            {
                var ReviewQuery = _dbContext.reviews.AsQueryable().Where(o => o.Id == item.Id).ToList();
                foreach (var items in ReviewQuery)
                {
                    if (items != null)
                    {
                        int? sum = items.Star;
                        numbers.Add(sum);
                        countStar += 1;
                    }
                }

                var TotalSumm = (from x in numbers select x).Sum();
                float? averageStar = null;
                if (countStar != 0)
                {
                    averageStar = (TotalSumm / countStar);
                }
                else if (countStar == 0)
                {
                    averageStar = null;
                }
                restuls.Add(new BookDto()
                {
                    Id = item.Id,
                    BookName = item.BookName,
                    Author = item.Author,
                    TypeOfBook = item.TypeOfBook,
                    Price = item.Price,
                    Image = item.Image,
                    TotalStar = averageStar,
                    DiscountPercent = item.DiscountPercent,
                    TotalLike = item.TotalLike,
                    TotalSales = item.TotalSales,
                    Describe = item.Describe,
                });
            }

            restul.LatestProducts = restuls.Take(30).OrderByDescending(item => item.Id).ToList();
            restul.PopularProducts = restuls.OrderBy(x => Guid.NewGuid()).Take(30).ToList();
            restul.SellingProducts = restuls.Take(30).OrderByDescending(item => item.TotalSales).ToList();
            restul.TopRatedProducts = restuls.Take(30).OrderByDescending(item => item.TotalStar).ToList();

            return new PageResultDto<BookDailySuggestDto>
            {
                Item = restul,
                TotalItem = 0,
            };
        }

        public PageResultDto<List<BookDto>> FindAllBookUser(FilterBookDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var bookQuery = _dbContext.books.AsQueryable();
            if (input.Keyword != null)
            {
                bookQuery = bookQuery.Where(s => s.BookName != null && s.BookName.Contains(input.Keyword));
            }
            int totalItem = bookQuery.Count();

            var restul = new List<BookDto>();
            var numbers = new List<int?>();
            var countStar = 0;
            foreach (var itemStart in bookQuery)
            {
                var ReviewQuery = _dbContext.reviews.AsQueryable().Where(o => o.Id == itemStart.Id).ToList();
                foreach (var items in ReviewQuery)
                {
                    if (items != null)
                    {
                        int? sum = items.Star;
                        numbers.Add(sum);
                        countStar += 1;
                    }
                }

                var TotalSumm = (from x in numbers select x).Sum();
                if (input.PageSize != -1)
                {
                    bookQuery = bookQuery.Skip(input.Skip).Take(input.PageSize);
                }
                else
                {
                    bookQuery = bookQuery.Skip(input.PageSize * (input.PageIndex - 1)).Take(input.PageSize);
                }
                float? averageStar = null;
                if (countStar != 0)
                {
                    averageStar = (TotalSumm / countStar);
                }
                else if (countStar == 0)
                {
                    averageStar = null;
                }

                foreach (var item in bookQuery)
                {
                    restul.Add(new BookDto()
                    {
                        Id = item.Id,
                        BookName = item.BookName,
                        Author = item.Author,
                        TypeOfBook = item.TypeOfBook,
                        Price = item.Price,
                        Image = item.Image,
                        TotalStar = averageStar,
                    });
                };
            }

            return new PageResultDto<List<BookDto>>
            {
                Item = restul,
                TotalItem = totalItem
            };
        }

        public void Like(Like input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var LikeFind = _dbContext.likes.FirstOrDefault(l => l.UserId == userid && l.BookId == input.BookId);
            var bookFind = _dbContext.books.FirstOrDefault(b => b.Id == LikeFind.BookId);
            if (LikeFind == null)
            {
                _dbContext.likes.Add(new Like()
                {
                    UserId = userid,
                    BookId = input.BookId,
                });
                bookFind.TotalLike += 1;
            }
            else if (LikeFind != null)
            {
                _dbContext.likes.Remove(LikeFind);
                bookFind.TotalLike -= 1;
            }
            _dbContext.SaveChanges();
        }

        public void ListImageBook(int IdBook, List<string> iamge)
        {
            foreach (var item in iamge)
            {

                _dbContext.listImageBooks.Add(new ListImageBook()
                {
                    IdBook = IdBook,
                    Image = item,
                });
            }
            _dbContext.SaveChanges();
        }

        public void Reviews(Review input, int DetailId)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var billdetail = _dbContext.billDetails.FirstOrDefault(o => o.Id == DetailId);
            billdetail.IsReview = "Y";

            var review = _dbContext.reviews.Add(new Review()
            {
                UserId = userid,
                BookId = input.BookId,
                Text = input.Text,
                Star = input.Star,
                CreateBy = userName,
                CreateDate = DateTime.Now,
                Image = input.Image
            });

            _dbContext.SaveChanges();
        }

        public List<ReviewsDto> FindAllReview(int idBook)
        {
            List<ReviewsDto> reviews = new List<ReviewsDto>();
            var review = _dbContext.reviews.Where(s => s.BookId == idBook).ToList();
            foreach (var item in review)
            {
                var userName = _dbContext.users.FirstOrDefault(u => u.Id == item.UserId);
                var items = _mapper.Map<ReviewsDto>(item);
                items.CustomerName = userName.CustomerName;
                items.ImageUser = userName.Image;
                reviews.Add(items);
            }
            return reviews;
        }


        public List<Discount> FindAllDiscount()
        {
            var cartDetailQuery = _dbContext.discounts.AsQueryable().Where(o => o.Active == "Y").ToList();
            return cartDetailQuery;
        }

        public List<String> FuzzySreach(string query)
        {
            List<string> listAuthor = new List<string>();
            List<string> listSreach = new List<string>();
            var book = _dbContext.books.AsQueryable();
            foreach (var item in book)
            {
                listAuthor.Add(item.Author);
            }

            foreach (string author in listAuthor)
            { // đầu vào ngưỡng threshold, có thể điều chỉnh, càng nhỏ thì khoảng cách chấp nhận được càng lớn
                if (FuzzyMatch(author, query, 0.1))
                {
                    listSreach.Add(author);
                }
            }
            return listSreach;
        }

        static bool FuzzyMatch(string input, string pattern, double threshold)
        {
            int maxDistance = Convert.ToInt32(pattern.Length * (1 - threshold)); // khoảng cách tối đa chấp nhận được
            return LevenshteinDistance(input, pattern) <= maxDistance;           // khoảng cách a b <= khoảng cách chấp nhận được
        }

        static int LevenshteinDistance(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;    // giống = 0, khác = 1
                    dp[i, j] = Math.Min(
                        dp[i - 1, j] + 1,             //insert
                        Math.Min(
                            dp[i, j - 1] + 1,         //deleted
                            dp[i - 1, j - 1] + cost   //replace
                        )
                    );
                }
            }
            var x = dp[a.Length, b.Length];
            return dp[a.Length, b.Length];  // trả về khoảng cách giữa ma trận a và b
        }
    }
}
