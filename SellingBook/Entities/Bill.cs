using SellingBook.Constants;
using SellingBook.Dtos.BillDto.BillDetailDto;
using SellingBook.Validation;

namespace SellingBook.Entities
{
    public class Bill
    {
        
        public int Id { get; set; }
        public int IdCart { get; set; }
        public int IdUser { get; set; }
        public float TotalPrice { get; set; }
        /// <summary>
        /// hinh thuc thanh toan
        /// </summary>
        [IntegerRange(AllowableValues = new int[] { BookDelivery.DeliverySave, BookDelivery.DeliveryFast, BookDelivery.DeliveryExpress })]
        public int Delivery { get; set; }
        [IntegerRange(AllowableValues = new int[] { Payment.CardBank, Payment.COD })]
        public int Payments { get; set; }
        public List<BillDetailsDto> BillDetail { get; set; }
        public int Status { get; set; }
        public int AddressId { get; set; }
    }
}
