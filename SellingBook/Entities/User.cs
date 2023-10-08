namespace SellingBook.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerName { get; set; }
        public char? Sex { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? UserType { get; set; }
        public string? Image { get; set; }
    }
}
