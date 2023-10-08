namespace APIBoSellingBookokSaling.Dtos.UsersDto
{
    public class CreateUserDto
    {
        private string _username;
        public string UserName
        {
            get => _username;
            set => _username = value?.Trim();
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }
        public string CustomerName { get; set; }
        public char Sex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int UserType { get; set; }
    }
}
