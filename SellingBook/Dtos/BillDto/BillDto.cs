using SellingBook.Dtos.BillDto.BillDetailDto;
using SellingBook.Dtos.BooksDto;
using SellingBook.Entities;

namespace SellingBook.Dtos.BillDto
{
    public class BillDto
    {
        public float TotalPrice { get; set; }
        public List<BillDetailsDto> billDetails { get; set; }
    }
}
