namespace TomasosPizzeria.Data.DTOs.DishDTOs
{
    public class UpdateDishDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public List<int> IngredientIds { get; set; } = new();
    }

}
