namespace TomasosPizzeria.Data.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
