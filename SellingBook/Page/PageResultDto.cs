namespace SellingBook.Page
{
    public class PageResultDto<T>
    {
        public T Item { get; set; }
        public int TotalItem { get; set; }
    }
}
