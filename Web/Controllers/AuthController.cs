using System;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> Login([FromBody] LoginRequest request)
        {
            // 1. Validar modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Buscar usuario por correo
            var usuario = await _usuarioService.GetByCorreoAsync(request.Correo);
            if (usuario == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // 3. Verificar contraseña (compatibilidad con texto plano y hash)
            if (!VerifyPassword(request.Contrasena, usuario.Contrasena))
                return Unauthorized(new { message = "Credenciales inválidas" });

            // 4. Generar token JWT
            var token = GenerateJwtToken(usuario);

            // 5. Retornar respuesta (sin contraseña)
            return Ok(new {
                token,
                user = new {
                    id = usuario.Id,
                    correo = usuario.Correo,
                    tipo_usuario = usuario.TipoUsuario,
                    clinica_id = usuario.ClinicaId
                }
            });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim("role", usuario.TipoUsuario),
                new Claim("clinicaId", usuario.ClinicaId?.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string CreatePasswordHash(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashOfInput = CreatePasswordHash(password);
            return hashOfInput == storedHash;
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Si la contraseña almacenada no parece un hash (longitud < 64), comparar directamente
            if (storedPassword.Length < 64)
            {
                return inputPassword == storedPassword;
            }
            
            // Si parece un hash, verificar con hash
            var hashedInput = CreatePasswordHash(inputPassword);
            return hashedInput == storedPassword;
        }
    }

    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}