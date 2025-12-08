namespace PhoneInventory
{
    public class PhoneDTO
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool IsAvailable { get; set; }

        public PhoneDTO() { }
        public PhoneDTO(Phone phone)
        {
            Id = phone.Id;
            Make = phone.Make;
            Model = phone.Model;
            IsAvailable = phone.IsAvailable;
        }
    }
}
