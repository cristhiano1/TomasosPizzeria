using TomasosPizzeria.Data.DTOs.UserDTOs;

namespace TomasosPizzeria.Data.Interfaces
{
    public interface IUserService
    {


        Task<UserDetailsDto> GetUserByIdAsync(int userId);
    }
}
