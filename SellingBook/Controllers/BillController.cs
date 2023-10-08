using SellingBook.Dtos.BillDto;
using SellingBook.Dtos.Discounts;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using Microsoft.AspNetCore.Mvc;

namespace SellingBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : SellingBook
    {
        private readonly IBillServices _billServices;

        public BillController(
        IBillServices billServices,
        ILogger<BillController> logger) : base(logger)
        {
            _billServices = billServices;
        }
        // GET api/<UserController>/5
        [HttpGet("find-bill/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var user = _billServices.FindById(id);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm tất cả item trong bill của user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-bill")]
        public APIResponse Find(int Status)
        {
            try
            {
                var user = _billServices.Find(Status);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm tất cả bill
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-bill-all")]
        public APIResponse FindAll(FilterDto input)
        {
            try
            {
                var user = _billServices.FindAll(input);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update bill
        /// </summary>
        /// <param name="input"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update-bill/{id}")]
        public APIResponse UpdateBill(CreateBillDto input, int id)
        {
            try
            {
                _billServices.UpdateBill(input, id);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update trang thai giao hang
        /// </summary>
        /// <param name="idBill"></param>
        /// <returns></returns>
        [HttpPut("update-delivery/{idBill}")]
        public APIResponse Delivery(int idBill)
        {
            try
            {
                _billServices.Status(idBill);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa bill
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-bill/{id}")]
        public APIResponse Deleted(int id)
        {
            try
            {
                _billServices.Deleted(id);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tạo mới list bill
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ListIdBook"></param>
        /// <returns></returns>
        [HttpPost("create-bill")]
        public APIResponse CreateBill([FromForm] List<int> ListIdBook, [FromForm] string name, int payment, int delivery, int adressId)
        {
            try
            {
                _billServices.CreateBill(ListIdBook, name, payment, delivery, adressId);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///  thêm mới discount
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create-discount")]
        public APIResponse CreateDiscount(CreateDiscountDto input)
        {
            try
            {
                _billServices.CreateDiscount(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update % discount
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-discount")]
        public APIResponse UpdateDiscount(UpdateDiscountDto input)
        {
            try
            {
                _billServices.UpdateDiscount(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// đổi trạng thái của discount có dùng được hay không
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-discount-status")]
        public APIResponse DiscounStatus(UpdateDiscountDto input)
        {
            try
            {
                _billServices.DiscounStatus(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }
    }
}
