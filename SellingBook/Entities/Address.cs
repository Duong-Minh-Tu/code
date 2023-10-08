namespace SellingBook.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string DetailAddress { get; set; }
        public char IsDefaul { get; set; }
    }
}
