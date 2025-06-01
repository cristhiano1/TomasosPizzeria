using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TomasosPizzeria.Context;
using TomasosPizzeria.Data.DTOs.UserDTOs;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("El nombre de usuario ya existe.");

            using var hmac = new System.Security.Cryptography.HMACSHA512();

            var passwordBytes = Encoding.UTF8.GetBytes(dto.Password);
            var passwordHash = hmac.ComputeHash(passwordBytes);
            var passwordSalt = hmac.Key;

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = passwordSalt,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                 Role = dto.IsAdmin ? "Admin" : "RegularUser", // Asignar rol
                BonusPoints = 0
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _tokenService.CreateToken(user);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) return Unauthorized("Usuario no encontrado.");

            using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
            var storedHash = Convert.FromBase64String(user.PasswordHash);

            if (!computedHash.SequenceEqual(storedHash))
                return Unauthorized("Contraseña incorrecta.");

            var token = _tokenService.CreateToken(user);

            return Ok(new { token });
        }

    }
}