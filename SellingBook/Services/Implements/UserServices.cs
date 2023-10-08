using SellingBook.Constants;
using SellingBook.DbContexts;
using SellingBook.Dtos.UsersDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using APIBoSellingBookokSaling.Dtos.UsersDto;

namespace SellingBook.Services.Implements
{
    public class UserServices : IUserServices
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public UserServices(
            ApplicationDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContext = httpContext;
        }

        public void Create(LoginUserDto input)
        {
            if (_dbContext.users.Any(u => u.UserName == input.UserName))
            {
                throw new UserFriendlyException($"Tên tài khoản \"{input.UserName}\" đã tồn tại");
            }
            var add = _dbContext.users.Add(new User()
            {
                CustomerName = input.CustomerName,
                UserName = input.UserName,
                Password = CommonUtils.CreateMD5(input.Password),
                Email = input.Email
            });
            _dbContext.SaveChanges();
            var idUser = add.Entity.Id;
            _dbContext.carts.Add(new Cart()
            {
                IdUser = idUser,
            });
            _dbContext.SaveChanges();
        }

        public void CreateListUser(List<CreateUserDto> input)
        {

            foreach (var user in input)
            {
                if (_dbContext.users.Any(u => u.UserName == user.UserName))
                {
                    throw new UserFriendlyException($"Tên tài khoản \"{user.UserName}\" đã tồn tại");
                }
                _dbContext.users.Add(new User
                {
                    UserName = user.UserName,
                    Password = CommonUtils.CreateMD5(user.Password),
                });
            }
            _dbContext.SaveChanges();
        }

        public string Login(LoginUserDto input)
        {
            var user = _dbContext.users.FirstOrDefault(u => u.UserName == input.UserName);
            if (user == null)
            {
                throw new UserFriendlyException($"Tài khoản \"{input.UserName}\" không tồn tại");
            }

            if (CommonUtils.CreateMD5(input.Password) == user.Password)
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(CustomClaimTypes.UserType, user.UserType.ToString(), ClaimValueTypes.Integer32)
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddSeconds(_configuration.GetValue<int>("JWT:Expires")),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                throw new UserFriendlyException($"Mật khẩu không chính xác");
            }
        }

        public User FindById(int id)
        {
            var userQuery = _dbContext.users.AsQueryable();
            var userFind = userQuery.FirstOrDefault(s => s.Id == id);
            if (userFind == null)
            {
                throw new UserFriendlyException("khong tim thay tai khoan");
            }
            return userFind;
        }

        public PageResultDto<List<User>> FindAll(FilterDto input)
        {
            var userQuery = _dbContext.users.AsQueryable();
            if (input.Keyword != null)
            {
                userQuery = userQuery.Where(s => s.UserName != null && s.UserName.Contains(input.Keyword));
            }
            int totalItem = userQuery.Count();

            userQuery = userQuery.Skip(input.PageSize * (input.PageIndex - 1)).Take(input.PageSize);
            return new PageResultDto<List<User>>
            {
                Item = userQuery.ToList(),
                TotalItem = totalItem
            };
        }

        public int Deleted(int id)
        {
            var userQuery = _dbContext.users.AsQueryable();
            var userFind = userQuery.FirstOrDefault(s => s.Id == id);
            if (userFind == null)
            {
                throw new UserFriendlyException("khong tim thay hoc sinh");
            }
            _dbContext.users.Remove(userFind);
            _dbContext.SaveChanges();
            return 0;
        }

        public void ChangePassword(string password, string newPassword)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var user = _dbContext.users.FirstOrDefault(u => u.Id == userid);
            if (user == null)
            {
                throw new UserFriendlyException($"Tài khoản không tồn tại");
            }

            if (CommonUtils.CreateMD5(password) == user.Password)
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(CustomClaimTypes.UserType, user.UserType.ToString(), ClaimValueTypes.Integer32)
                };
            }
            else
            {
                throw new UserFriendlyException($"Mật khẩu không chính xác");
            }
            user.Password = CommonUtils.CreateMD5(newPassword);
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User input)
        {
            //var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var userQuery = _dbContext.users.AsQueryable();
            var userFind = userQuery.FirstOrDefault(s => s.Id == input.Id);
            if (userFind == null)
            {
                throw new UserFriendlyException("khong tim tai khoan");
            }
            userFind.Image = input.Image;
            userFind.CustomerName = input.CustomerName;
            userFind.Sex = input.Sex;
            userFind.Phone = input.Phone;
            userFind.Email = input.Email;
            _dbContext.SaveChanges();
        }
        
        public UserDto GetMyInfo()
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var userQuery = _dbContext.users.AsQueryable();
            var user = userQuery.FirstOrDefault(s => s.Id == userid);
            var address = _dbContext.address.FirstOrDefault(u => u.IdUser == userid && u.IsDefaul == 'Y');
            var result = new UserDto
            {
                Id = userid,
                UserName = user.UserName,
                Password = user.Password,
                CustomerName = user.CustomerName,
                Sex = user.Sex,
                Phone = user.Phone,
                Email = user.Email,
                Image = user.Image,
                AddressId = address?.Id,
                PhoneAddress = address?.Phone,
                NameAddress = address?.Name,
                DetailAddress = address?.DetailAddress,
            };
            return result;
        }

        public void ChangeDefaultAddress(int idAddressNew, int idAddressOld)
        {
            var addressNew = _dbContext.address.FirstOrDefault(a => a.Id == idAddressNew);
            var addressOld = _dbContext.address.FirstOrDefault(a => a.Id == idAddressOld);
            addressNew.IsDefaul = 'Y';
            addressOld.IsDefaul = 'N';
            _dbContext.SaveChanges();
        }

        public List<Address> GetAllAddress()
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var address = _dbContext.address.AsQueryable();
            return address.Where(o => o.IdUser == userid).OrderByDescending(o => o.IsDefaul).ToList();
        }

        public void updateAdress(Address input)
        {
            var address = _dbContext.address.FirstOrDefault(a => a.Id == input.Id);
            address.DetailAddress = input.DetailAddress;
            address.Name = input.Name;
            address.Phone = input.Phone;
            _dbContext.SaveChanges();
        }

        public void addAdress(Address input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var address = _dbContext.address.AsQueryable();
            var checkAddress =  address.FirstOrDefault(o => o.IdUser == userid && o.IsDefaul == 'Y');
            if(checkAddress != null)
            {
                _dbContext.address.Add(new Address()
                {
                    IdUser = userid,
                    Name = input.Name,
                    Phone = input.Phone,
                    DetailAddress = input.DetailAddress,
                    IsDefaul = 'N',
                });
            }
            else
            {
                _dbContext.address.Add(new Address()
                {
                    IdUser = userid,
                    Name = input.Name,
                    Phone = input.Phone,
                    DetailAddress = input.DetailAddress,
                    IsDefaul = 'Y',
                });
            }
            _dbContext.SaveChanges();
        }
    }
}