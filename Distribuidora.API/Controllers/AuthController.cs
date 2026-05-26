using Distribuidora.API.Data;
using Distribuidora.Shared.DTOs.Auth;
using Distribuidora.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AuthController(
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            User? existingUser =
                await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return BadRequest("El usuario ya existe.");
            }

            User user = new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                NombreCompleto = dto.NombreCompleto
            };

            IdentityResult result =
                await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok("Usuario registrado");
        }
       
        


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Unauthorized("Usuario no existe");
            }

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!result)
            {
                return Unauthorized("Contraseña incorrecta");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email!),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtKey"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}