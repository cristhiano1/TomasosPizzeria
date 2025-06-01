using TomasosPizzeria.Data.DTOs.UserDTOs;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Service
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
      

        public UserService(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<UserDetailsDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new UserDetailsDto
            {
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                BonusPoints = user.BonusPoints
            };
        }
    }
}
