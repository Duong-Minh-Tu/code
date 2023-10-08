namespace SellingBook.Entities
{
    public class HistoryBill
    {
        public int Id { get; set; }
        public int BillDetailId { get; set; }
        public DateTime? BillDate { get; set; }
    }
}
