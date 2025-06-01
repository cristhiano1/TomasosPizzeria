using Microsoft.EntityFrameworkCore;
using System;
using TomasosPizzeria.Context;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Data.Repos



{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Orders)
                .ThenInclude(o => o.Items) // CORREGIDO
                .ThenInclude(oi => oi.Dish)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Orders)
                .ThenInclude(o => o.Items) // CORREGIDO
                .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        //public async Task<User?> GetUserByUsernameAsync(string username)
        //{
        //    return await _context.Users
        //        .Include(u => u.Orders)
        //        .ThenInclude(o => o.Items) // CORREGIDO
        //        .ThenInclude(oi => oi.Dish)
        //        .FirstOrDefaultAsync(u => u.Username == username);
        //}

        //public async Task<User?> GetUserByEmailAsync(string email)
        //{
        //    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        //}

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
