namespace TomasosPizzeria.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "RegularUser"; // Default role
        public int BonusPoints { get; set; } = 0;
        public byte[] PasswordSalt { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

