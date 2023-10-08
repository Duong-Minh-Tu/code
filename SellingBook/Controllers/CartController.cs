using SellingBook.Dtos.CartsDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using Microsoft.AspNetCore.Mvc;

namespace SellingBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : SellingBook
    {
        private readonly ICartServices _cartServices;

        public CartController(
        ICartServices cartServices,
        ILogger<CartController> logger) : base(logger)
        {
            _cartServices = cartServices;
        }
        // GET api/<UserController>/5
        [HttpGet("find-cart-by-user")]
        public APIResponse Find()
        {
            try
            {
                var user = _cartServices.Find();
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm item vào cart
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        [HttpPut("update-cart")]
        public APIResponse UpdateCart([FromBody] List<ListBookCart> add)
        {
            try
            {
                _cartServices.UpdateCart(add);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("delete-cart/{id}")]
        public APIResponse Deleted(int id)
        {
            try
            {
                _cartServices.Deleted(id);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa item trong cart
        /// </summary>
        /// <param name="listIdBook"></param>
        /// <returns></returns>
        // DELETE api/<UserController>/5
        [HttpDelete("delete-item-cart")]
        public APIResponse DeletedItemCart([FromQuery] List<int> listIdBook)
        {
            try
            {
                _cartServices.DeletedItemCart(listIdBook);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }
    }
}
