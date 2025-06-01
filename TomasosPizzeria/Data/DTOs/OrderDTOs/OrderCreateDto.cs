namespace TomasosPizzeria.Data.DTOs.OrderDTOs
{
    public class OrderCreateDto
    {
        public List<int> DishIds { get; set; } = new List<int>(); // Lista de IDs de platos pedidos
        public decimal TotalPrice { get; set; }
    }
}
