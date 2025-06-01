namespace TomasosPizzeria.Data.DTOs.OrderDTOs
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string CategoryName { get; set; } = null!; // Por si quieres mostrar que es "Pizza", "Pasta", etc.
        public List<string> Ingredients { get; set; } = new(); // Lista simple de nombres de ingredientes
    }
}
