using TomasosPizzeria.Data.Entities;

namespace TomasosPizzeria.Data.Interfaces
{
    public interface IDishRepository
    {
        Task<IEnumerable<Dish>> GetAllDishesAsync();
        Task<Dish?> GetDishByIdAsync(int id);
        Task AddDishAsync(Dish dish);
        void UpdateDish(Dish dish);
        Task DeleteDishAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
