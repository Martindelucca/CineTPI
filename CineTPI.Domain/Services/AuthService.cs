using CineTPI.Domain.DTOs;
using CineTPI.Domain.Models;
using CineTPI.Domain.Interfaces;
using Microsoft.Extensions.Configuration; 
using Microsoft.IdentityModel.Tokens; 
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims; 
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CineTPI.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IClienteRepository clienteRepository, IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _configuration = configuration; // Inyectamos la config para leer la Clave Secreta
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            // 1. Buscar al usuario por su NroDoc

            var nroDocLimpio = loginRequest.NroDoc.Trim();
            var cliente = await _clienteRepository.GetClienteByDocAsync(nroDocLimpio);

            if (cliente == null)
            {
                return null; // Usuario no existe
            }

            // 2. Verificar la contraseña usando BCrypt
            var inputPassword = loginRequest.Password.Trim();
            var dbHash = cliente.PasswordHash.Trim();

            bool esPasswordValido = BCrypt.Net.BCrypt.Verify(inputPassword, dbHash);

            if (!esPasswordValido)
            {
                return null; // Contraseña incorrecta
            }

            // 3. ¡Usuario válido! Generar el Token JWT
            var token = GenerateJwtToken(cliente);

            // 4. Devolver el DTO de respuesta
            return new LoginResponseDto
            {
                CodCliente = cliente.CodCliente,
                NombreCompleto = $"{cliente.Nombre} {cliente.Apellido}",
                Token = token
            };
        }

        private string GenerateJwtToken(Cliente cliente)
        {
            // Leemos la configuración del appsettings.json
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Los "Claims" son la información del usuario que va DENTRO del token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, cliente.CodCliente.ToString()), // ID del usuario
                new Claim(JwtRegisteredClaimNames.Name, cliente.Nombre),
                new Claim(JwtRegisteredClaimNames.Email, cliente.NroDoc), // Usamos NroDoc como "email" o "user"
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único del token
            };

            // Si es admin, le agregamos el "Rol" de Admin
            if (cliente.EsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Cliente"));
            }

            // Creamos el token
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(8), // El "pase" vence en 8 horas
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}