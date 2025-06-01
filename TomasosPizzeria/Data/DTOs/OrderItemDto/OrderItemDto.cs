namespace TomasosPizzeria.Data.DTOs.OrderItemDto
{
    public class OrderItemDto
    {
        public int DishId { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }
        public int Quantity { get; set; }
    }

}
