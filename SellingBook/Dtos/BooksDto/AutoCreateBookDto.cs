namespace SellingBook.Dtos.BooksDto
{
    public class AutoCreateBookDto
    {
        private string _bookName;
        public string BookName
        {
            get => _bookName;
            set => _bookName = value?.Trim();
        }

        // tac gia
        private string _author;
        public string Author
        {
            get => _author;
            set => _author = value?.Trim();
        }
        public string BookCode { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
