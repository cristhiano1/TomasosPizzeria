using TomasosPizzeria.Data.Entities;

namespace TomasosPizzeria.Data.Interfaces
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient?> GetIngredientByIdAsync(int id);
        Task AddIngredientAsync(Ingredient ingredient);
        void UpdateIngredient(Ingredient ingredient);
        Task DeleteIngredientAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
