using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CineTPI.Domain.DTOs;
using CineTPI.Domain.Models;
using CineTPI.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CineTPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepository _peliculaRepository;

        public PeliculasController(IPeliculaRepository peliculaRepository)
        {
            _peliculaRepository = peliculaRepository;
        }



        // Obtiene una lista simple de TODAS las películas
        [HttpGet]
        public async Task<IActionResult> GetPeliculas()
        {
            var peliculas = await _peliculaRepository.GetAllAsync();



            var peliculasDto = peliculas.Select(p => new PeliculaSimpleDto
            {
                IdPelicula = p.IdPelicula,
                Titulo = p.Titulo
            });


            return Ok(peliculasDto);
        }


        // Obtiene solo las películas que tienen funciones activas
        [HttpGet("cartelera")] 
        public async Task<IActionResult> GetPeliculasEnCartelera()
        {
            var peliculas = await _peliculaRepository.GetPeliculasEnCarteleraAsync();


            var peliculasDto = peliculas.Select(p => new PeliculaSimpleDto
            {
                IdPelicula = p.IdPelicula,
                Titulo = p.Titulo
            });

            return Ok(peliculasDto);
        }


        // Obtiene UNA película por su ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPelicula(int id)
        {
            var pelicula = await _peliculaRepository.GetByIdAsync(id);

            if (pelicula == null)
            {

                return NotFound();
            }


            return Ok(pelicula);
        }


        // Crea una nueva película
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePelicula([FromBody] PeliculaCreateDTO peliculaDto)
        {
            if (peliculaDto == null)
            {
                return BadRequest(); 
            }


            var pelicula = new Pelicula
            {
                Titulo = peliculaDto.Titulo,
                Descripcion = peliculaDto.Descripcion,
                FechaLanzamiento = peliculaDto.FechaLanzamiento,
                IdPais = peliculaDto.IdPais,
                IdFormatoPelicula = peliculaDto.IdFormatoPelicula,
                IdClasificacion = peliculaDto.IdClasificacion,
                IdDistribuidor = peliculaDto.IdDistribuidor,
                IdDirector = peliculaDto.IdDirector
            };

            await _peliculaRepository.AddAsync(pelicula);

            return CreatedAtAction(nameof(GetPelicula), new { id = pelicula.IdPelicula }, pelicula);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePelicula(int id, [FromBody] Pelicula peliculaActualizada)
        {
            if (id != peliculaActualizada.IdPelicula)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo");
            }


            await _peliculaRepository.UpdateAsync(peliculaActualizada);

            return NoContent();
        }
        // Borra una película
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            var pelicula = await _peliculaRepository.GetByIdAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            await _peliculaRepository.DeleteAsync(id);

            return NoContent();
        }

    }
}
