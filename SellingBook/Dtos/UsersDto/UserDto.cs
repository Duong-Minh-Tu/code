namespace SellingBook.Dtos.UsersDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerName { get; set; }
        public char? Sex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int UserType { get; set; }
        public string Image { get; set; }
        public int? AddressId { get; set; }
        public string NameAddress { get; set; }
        public string PhoneAddress { get; set; }
        public string DetailAddress { get; set; }
    }
}
