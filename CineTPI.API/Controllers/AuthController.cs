using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        return Ok(loginResponse);
    }
}