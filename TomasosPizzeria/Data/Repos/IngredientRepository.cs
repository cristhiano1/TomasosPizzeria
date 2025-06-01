using Microsoft.EntityFrameworkCore;
using TomasosPizzeria.Context;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Data.Repos
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly AppDbContext _context;

        public IngredientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients
                .Include(i => i.Dishes)  // si quieres cargar la relación con Dishes
                .ToListAsync();
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(int id)
        {
            return await _context.Ingredients
                .Include(i => i.Dishes)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
        }

        public void UpdateIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
        }

        public async Task DeleteIngredientAsync(int id)
        {
            var ingredient = await GetIngredientByIdAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
