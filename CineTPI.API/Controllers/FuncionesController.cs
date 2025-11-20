using CineTPI.Domain;  
using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using CineTPI.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        // POST: api/funciones
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearFuncion([FromBody] FuncionCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            try
            {
                var nuevaFuncion = await _funcionRepository.CreateFuncionAsync(dto);

                return CreatedAtAction(
                    nameof(GetFuncionesPorPelicula),
                    new { idPelicula = dto.IdPelicula },
                    nuevaFuncion
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/funciones/5
        [HttpDelete("{idFuncion}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFuncion(int idFuncion)
        {
            try
            {
                var resultado = await _funcionRepository.DeleteFuncionAsync(idFuncion);

                if (!resultado)
                    return BadRequest("La función no puede eliminarse porque tiene reservas asociadas.");

                return NoContent(); // Eliminada correctamente
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}


