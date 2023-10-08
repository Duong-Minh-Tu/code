using SellingBook.Dtos.UsersDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using Microsoft.AspNetCore.Mvc;


namespace SellingBook.Controllers
{
    //[Authorize]
    //[AuthenticationFilter(UserTypes.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : SellingBook
    {
        private readonly IUserServices _userService;

        public UserController(
            IUserServices userService,
            ILogger<UserController> logger) : base(logger)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public APIResponse Create(LoginUserDto input)
        {
            try
            {
                _userService.Create(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("create-address")]
        public APIResponse addAdress(Address input)
        {
            try
            {
                _userService.addAdress(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("login")]
        public APIResponse Login(LoginUserDto input)
        {
            try
            {
                string token = _userService.Login(input);
                return new APIResponse(token, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-user-by-id/{id}")]
        public APIResponse FindByUserId(int id)
        {
            try
            {
                var user = _userService.FindById(id);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-user")]
        public APIResponse FindAll([FromQuery] FilterDto input)
        {
            try
            {
                var user = _userService.FindAll(input);
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }


        [HttpGet("get-info-user")]
        public APIResponse GetMyInfo()
        {
            try
            {
                var user = _userService.GetMyInfo();
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("get-address")]
        public APIResponse GetAllAddress()
        {
            try
            {
                var user = _userService.GetAllAddress();
                return new APIResponse(user, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update-user")]
        public APIResponse UpdateUser([FromBody] User input)
        {
            try
            {
                _userService.UpdateUser(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("change-password-user/{id}")]
        public APIResponse ChangePassword(string password, string newPassword)
        {
            try
            {
                _userService.ChangePassword(password, newPassword);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }


        [HttpPut("change-default-address")]
        public APIResponse ChangeDefaultAddress(int idAddressNew, int idAddressOld)
        {
            try
            {
                _userService.ChangeDefaultAddress( idAddressNew, idAddressOld);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update-address")]
        public APIResponse updateAdress(Address input)
        {
            try
            {
                _userService.updateAdress(input);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("delete-user/{id}")]
        public APIResponse DeleteUser(int id)
        {
            try
            {
                _userService.Deleted(id);
                return new APIResponse(null, 200, "Ok");
            }
            catch (UserFriendlyException ex)
            {
                return OkException(ex);
            }
        }
    }
}
