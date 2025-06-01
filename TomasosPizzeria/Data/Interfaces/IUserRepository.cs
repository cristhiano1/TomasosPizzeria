using TomasosPizzeria.Data.Entities;

namespace TomasosPizzeria.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        //Task<User?> GetUserByUsernameAsync(string username);
        //Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        void UpdateUser(User user);
        Task DeleteUserAsync(int id);
        Task<bool> SaveChangesAsync();


    }
}
