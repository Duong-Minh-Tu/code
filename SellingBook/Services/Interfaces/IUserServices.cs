using APIBoSellingBookokSaling.Dtos.UsersDto;
using SellingBook.Dtos.UsersDto;
using SellingBook.Entities;
using SellingBook.Page;

namespace SellingBook.Services.Interfaces
{
    public interface IUserServices
    {
        void Create(LoginUserDto input);
        string Login(LoginUserDto input);
        User FindById(int id);
        PageResultDto<List<User>> FindAll(FilterDto input);
        void ChangePassword(string password, string newPassword);
        public int Deleted(int id);
        public void UpdateUser(User input);
        public UserDto GetMyInfo();
        void ChangeDefaultAddress(int idAddressNew, int idAddressOld);
        List<Address> GetAllAddress();
        void updateAdress(Address input);
        void addAdress(Address input);
    }
}
