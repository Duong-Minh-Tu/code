using SellingBook.Dtos.BillDto;
using SellingBook.Dtos.BillDto.BillDetailDto;
using SellingBook.Dtos.CartsDto;
using SellingBook.Dtos.Discounts;
using SellingBook.Entities;
using SellingBook.Page;

namespace SellingBook.Services.Interfaces
{
    public interface IBillServices
    {
        void CreateBill(List<int> ListIdBook, string name, int payment, int delivery, int addressId);
        Bill FindById(int id);
        int Deleted(int id);
        void UpdateBill(CreateBillDto input, int id);
        PageResultDto<List<BillDto>> FindAll(FilterDto input);
        List<BillDto> Find(int Status);
        void Status(int idBill);
        void CreateDiscount(CreateDiscountDto input);
        void UpdateDiscount(UpdateDiscountDto input);
        void DiscounStatus(UpdateDiscountDto input);
    }
}
