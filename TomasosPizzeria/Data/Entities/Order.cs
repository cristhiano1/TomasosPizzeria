namespace TomasosPizzeria.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }
}
 