using SellingBook.Entities;

namespace SellingBook.Dtos.BillDto.BillDetailDto
{
    public class BillDetailsDto
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int IdBook { get; set; }
        // tổng giá
        public float TotalPrice { get; set; }
        // tên quyển sách
        public string BookName { get; set; }
        // số lượng
        public int Quantity { get; set; }
        // ngày tạo
        public DateTime CreateDate { get; set; }
        // người tạo
        public string CreateBy { get; set; }
        public string IsReview { get; set; }
        //public byte[] ImageBook { get; set; }
        public string ImageBook { get; set; }
        public Book book { get; set; }
    }
}
