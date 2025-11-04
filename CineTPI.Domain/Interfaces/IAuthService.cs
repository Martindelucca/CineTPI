using CineTPI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.Interfaces
{
    public interface IAuthService
    {
        // Intenta loguear al usuario.
        // Devuelve un DTO con el token si es exitoso, o null si falla.
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
    }
}
