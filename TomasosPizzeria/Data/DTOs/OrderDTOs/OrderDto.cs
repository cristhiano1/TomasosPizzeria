namespace TomasosPizzeria.Data.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<DishDto> Dishes { get; set; } = new List<DishDto>();
    }
}
