using SellingBook.Dtos.BillDto;
using SellingBook.Dtos.BillDto.BillDetailDto;
using SellingBook.Entities;
using SellingBook.Page;

namespace SellingBook.Services.Interfaces
{
    public interface IBillDetailServices
    {
        BillDetail FindById(int id);
        int Deleted(int id);
        void UpdateBillDetail(CreateBillDetailDto input, int id);
        PageResultDto<List<BillDetail>> FindAll(FilterDto input);
    }
}
