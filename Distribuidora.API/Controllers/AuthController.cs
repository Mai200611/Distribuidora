using Distribuidora.API.Helpers;
using Distribuidora.Shared.DTOs.Auth;
using Distribuidora.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMailHelper _mailHelper;
        private readonly string _container = "users";

        public AuthController(UserManager<User> userManager, IConfiguration configuration,
            IFileStorage fileStorage, IMailHelper mailHelper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _mailHelper = mailHelper;
        }

        // GET usuario actual
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Get()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity!.Name!);
            if (user == null) return NotFound();
            return Ok(new EditUserDTO
            {
                NombreCompleto = user.NombreCompleto,
                Photo = user.Photo
            });
        }

        // PUT editar usuario
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(EditUserDTO dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User.Identity!.Name!);
                if (user == null) return NotFound();

                if (!string.IsNullOrEmpty(dto.Photo))
                {
                    var photoBytes = Convert.FromBase64String(dto.Photo);
                    user.Photo = await _fileStorage.EditFileAsync(photoBytes, ".jpg", _container, user.Photo!);
                }

                user.NombreCompleto = dto.NombreCompleto;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return BadRequest(result.Errors.FirstOrDefault());

                var roles = await _userManager.GetRolesAsync(user);
                return Ok(BuildToken(user, roles.ToList()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST cambiar contraseña
        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(User.Identity!.Name!);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault()?.Description);

            return NoContent();
        }

        // GET confirmar email (link desde el correo)
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            token = token.Replace(" ", "+");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault());

            return NoContent();
        }

        // POST reenviar correo de confirmación
        [HttpPost("ResendToken")]
        public async Task<ActionResult> ResendToken(EmailDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return NotFound();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = $"{_configuration["UrlWEB"]}/ConfirmEmail?userId={user.Id}&token={token}";

            var response = _mailHelper.SendMail(user.NombreCompleto, user.Email!,
                "Distribuidora - Confirmación de cuenta",
                $"<h1>Distribuidora - Confirmación de cuenta</h1>" +
                $"<p>Para habilitar el usuario, haz clic en 'Confirmar Email':</p>" +
                $"<b><a href=\"{tokenLink}\">Confirmar Email</a></b>");

            if (response.IsSuccess) return NoContent();
            return BadRequest(response.Message);
        }

        // POST recuperar contraseña (envía email)
        [HttpPost("RecoverPassword")]
        public async Task<ActionResult> RecoverPassword(EmailDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenLink = $"{_configuration["UrlWEB"]}/ResetPassword?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            var response = _mailHelper.SendMail(user.NombreCompleto, user.Email!,
                "Distribuidora - Recuperación de contraseña",
                $"<h1>Distribuidora - Recuperación de contraseña</h1>" +
                $"<p>Para recuperar su contraseña, haz clic en 'Recuperar Contraseña':</p>" +
                $"<b><a href=\"{tokenLink}\">Confirmar Email</a></b>");

            if (response.IsSuccess) return NoContent();
            return BadRequest(response.Message);
        }

        // POST resetear contraseña
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
            if (result.Succeeded) return NoContent();
            return BadRequest(result.Errors.FirstOrDefault()?.Description);
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
            try
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

                // Enviar email de confirmación
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = $"{_configuration["UrlWEB"]}/ConfirmEmail?userId={user.Id}&token={token}";
                var mailResponse = _mailHelper.SendMail(user.NombreCompleto, user.Email!,
                    "Distribuidora - Confirmación de cuenta",
                    $"<h1>Distribuidora - Confirmación de cuenta</h1>" +
                    $"<p>Para habilitar el usuario, haz clic en 'Confirmar Email':</p>" +
                    $"<b><a href=\"{tokenLink}\">Confirmar Email</a></b>");

                if (!mailResponse.IsSuccess)
                    return BadRequest(mailResponse.Message);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " | " + ex.InnerException?.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Usuario no existe");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("El usuario no ha confirmado su email. Revisa tu correo.");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result) return Unauthorized("Contraseña incorrecta");

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

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new AuthResponseDTO
            {
                Ok = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
