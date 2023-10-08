using SellingBook.Dtos.BooksDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace SellingBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : SellingBook
    {
        private readonly IBookServices _bookServices;
        private readonly IWebHostEnvironment _webHostEnvironment = null;

        public BookController(
        IBookServices bookServices,
        ILogger<BookController> logger) : base(logger)
        {
            _bookServices = bookServices;
        }
        

        /// <summary>
        /// tìm kiếm sách theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-book/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var book = _bookServices.FindById(id);
                //book.Image = _bookServices.GetImage(Convert.ToBase64String(book.Image));
                return new APIResponse(book, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm tất cả sách, FiterPrice = 1 giá cao tới thấp, FiterPrice = 2 thấp tới cao, NewBook = 1 là theo sách mới nhất, TotalSales = 1 là số lượng bán nhiều nhất
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-book")]
        public APIResponse FindAll([FromQuery] FilterBookDto input)
        {
            try
            {
                var user = _bookServices.FindAll(input);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-book-daily-suggest")]
        public APIResponse FindAllDailySuggest([FromQuery] FilterBookDto input)
        {
            try
            {
                var user = _bookServices.FindAllDailySuggest(input);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm đánh giá sách theo id book
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        [HttpGet("find-review-by-id-book")]
        public APIResponse FindAll(int idBook)
        {
            try
            {
                var user = _bookServices.FindAllReview(idBook);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm tất cả sách của user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-book-user")]
        public APIResponse FindAllBookUser([FromQuery] FilterBookDto input)
        {
            try
            {
                var user = _bookServices.FindAllBookUser(input);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tim cac discout con hoat dong
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all-discount")]
        public APIResponse FindAllDiscount()
        {
            try
            {
                var discount = _bookServices.FindAllDiscount();
                return new APIResponse(discount, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }


        // PUT api/<UserController>/5
        [HttpPut("update-book/{id}")]
        public APIResponse UpdateBook(CreateBookDto input, int id)
        {
            try
            {
                _bookServices.UpdateBook(input, id);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("delete-book/{id}")]
        public APIResponse Deleted(int id)
        {
            try
            {
                _bookServices.Deleted(id);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        // use create FromBody
        //[HttpPost("create-book")]
        //public APIResponse CreateBook([FromForm] FileUpLoad fileObj, [FromForm] BookDto input)
        //{
        //    try
        //    {
        //        _bookServices.CreateBook(fileObj, input);
        //        return new APIResponse(null, 200, "Ok");
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        // use create FromBody
        [HttpPost("create-book")]
        public APIResponse AutoCreateBook()
        {
            try
            {
                _bookServices.AutoCreateBook();
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        // use create FromBody
        //[HttpPost("create-book")]
        //public IActionResult CreateBook([FromForm] CreateBookDto input)
        //{
        //    try
        //    {
        //        _bookServices.CreateBook(input);
        //        return Ok();
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("like")]
        public APIResponse Like([FromForm] Like input)
        {
            try
            {
                _bookServices.Like(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        //[HttpPost("reviews")]
        //public APIResponse Reviews([FromForm] List<IFormFile> files,[FromForm] Review input)
        //{
        //    try
        //    {
        //        List<IFormFile> g = new List<IFormFile>();
        //        foreach (var file in files)
        //        {
        //            using (var stream = new FileStream("tudz", FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //                g.Add(file);
        //            }
        //        }
        //        _bookServices.Reviews(input, g);
        //        return new APIResponse(null, 200, "Ok");
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        [HttpPost("reviews")]
        public APIResponse Reviews([FromForm] Review input, int DetailId)
        {
            try
            {
                _bookServices.Reviews(input, DetailId);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("FuzzySreach")]
        public APIResponse FuzzySreach(string query)
        {
            try
            {
                var data = _bookServices.FuzzySreach(query);
                return new APIResponse(data, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("list-image-book")]
        public APIResponse Reviews([FromForm] List<string> iamge, int IdBook)
        {
            try
            {
                _bookServices.ListImageBook(IdBook, iamge);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }
    }
}
