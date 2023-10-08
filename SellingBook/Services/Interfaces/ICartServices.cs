using SellingBook.Dtos.CartsDto;
using SellingBook.Entities;
using SellingBook.Page;

namespace SellingBook.Services.Interfaces
{
    public interface ICartServices
    {
        Cart Find();
        int Deleted(int id);
        void UpdateCart(List<ListBookCart> add);
        int DeletedItemCart(List<int> listIdBook);

    }
}
