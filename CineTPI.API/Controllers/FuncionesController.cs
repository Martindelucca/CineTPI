using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ¡Protegemos TODAS las rutas de este controlador!
    public class FuncionesController : ControllerBase
    {
        private readonly IFuncionRepository _funcionRepository;

        // Inyectamos el repositorio de Funciones
        public FuncionesController(IFuncionRepository funcionRepository)
        {
            _funcionRepository = funcionRepository;
        }

        // Endpoint para cumplir el requisito del frontend
        [HttpGet("pelicula/{idPelicula}")]
        public async Task<IActionResult> GetFuncionesPorPelicula(int idPelicula)
        {

            var funcionesDto = await _funcionRepository.GetFuncionesPorPeliculaAsync(idPelicula);

            // 2. Verificamos si la lista está vacía
            if (funcionesDto == null || !funcionesDto.Any())
            {
                return Ok(new List<FuncionDTO>());
            }

            // 3. Devolvemos la lista de DTOs
            return Ok(funcionesDto);
        }
    }
}

