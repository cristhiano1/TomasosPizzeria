using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TomasosPizzeria.Data.DTOs.UserDTOs;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepo, IUserService userService)
        {
            _userRepo = userRepo;
            _userService = userService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepo.GetAllUsersAsync();
            
            return Ok(users);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = dto.Password, // idealmente, aquí deberías hashearla
                Role = "Customer", // puedes asignar un rol por defecto si lo necesitas
                BonusPoints = 0
            };

            await _userRepo.AddUserAsync(user);
            await _userRepo.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto updatedUser)
        {
            var existing = await _userRepo.GetUserByIdAsync(id); // Obtener el usuario existente por ID
            if (existing == null) return NotFound();

            // Solo hasheamos si updatedUser.Password NO está vacío o nulo
            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                var passwordHasher = new PasswordHasher<object>();
                existing.PasswordHash = passwordHasher.HashPassword(null, updatedUser.Password);
            }

            existing.Email = updatedUser.Email;
            existing.PhoneNumber = updatedUser.PhoneNumber;
            // otros campos...

            _userRepo.UpdateUser(existing);
            await _userRepo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepo.DeleteUserAsync(id);
            await _userRepo.SaveChangesAsync();
            return NoContent();
        }
    }
}
