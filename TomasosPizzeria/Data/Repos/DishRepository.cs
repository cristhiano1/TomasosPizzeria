using Microsoft.EntityFrameworkCore;
using TomasosPizzeria.Context;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Data.Repos
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _context;

        public DishRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Ingredients)
                .ToListAsync();
        }

        public async Task<Dish?> GetDishByIdAsync(int id)
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDishAsync(Dish dish)
        {
            await _context.Dishes.AddAsync(dish);
        }

        public void UpdateDish(Dish dish)
        {
            _context.Dishes.Update(dish);
        }

        public async Task DeleteDishAsync(int id)
        {
            var dish = await GetDishByIdAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
