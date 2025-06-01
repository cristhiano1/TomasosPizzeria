using TomasosPizzeria.Data.DTOs.IngredientDTOs;

namespace TomasosPizzeria.Data.DTOs.DishDTOs
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public List<IngredientDto> Ingredients { get; set; } = new();
    }

}
