using Distribuidora.API.Data;
using Distribuidora.API.Helpers;
using Distribuidora.Shared.DTOs.Auth;
using Distribuidora.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly string _container = "users";

        public AuthController(UserManager<User> userManager, IConfiguration configuration, IFileStorage fileStorage)
        {
            _userManager = userManager;
            _configuration = configuration;
            _fileStorage = fileStorage;
        }

        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(ClaimTypes.Name)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Rol = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest("El usuario ya existe.");

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                NombreCompleto = dto.NombreCompleto
            };

            if (!string.IsNullOrEmpty(dto.Photo))
            {
                var photoBytes = Convert.FromBase64String(dto.Photo);
                user.Photo = await _fileStorage.SaveFileAsync(photoBytes, ".jpg", _container);
            }

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Role);
            return Ok(BuildToken(user, new List<string> { dto.Role }));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Usuario no existe");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result)
                return Unauthorized("Contraseña incorrecta");

            var roles = await _userManager.GetRolesAsync(user);
            return BuildToken(user, roles.ToList());
        }

        private AuthResponseDTO BuildToken(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("NombreCompleto", user.NombreCompleto)
            };

            if (!string.IsNullOrEmpty(user.Photo))
                claims.Add(new Claim("Photo", user.Photo));

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new AuthResponseDTO
            {
                Ok = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
