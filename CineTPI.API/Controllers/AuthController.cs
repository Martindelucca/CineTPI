using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        if (loginRequest == null)
        {
            return BadRequest("Request inválido.");
        }

        var loginResponse = await _authService.LoginAsync(loginRequest);

        if (loginResponse == null)
        {
            // Damos un error genérico por seguridad
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        // ¡Éxito! Devolvemos 200 OK con el token
        return Ok(loginResponse);
    }
}