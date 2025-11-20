using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CineTPI.Domain.DTOs;
using CineTPI.Domain.Models;
using CineTPI.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CineTPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // toda la API requiere estar logueado
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepository _peliculaRepository;

        public PeliculasController(IPeliculaRepository peliculaRepository)
        {
            _peliculaRepository = peliculaRepository;
        }

        // GET api/peliculas  - lista completa para ABMC
        [HttpGet]
        public async Task<IActionResult> GetPeliculas()
        {
            var peliculas = await _peliculaRepository.GetAllAsync();

            var peliculasDto = peliculas.Select(p => new PeliculaListDto
            {
                IdPelicula = p.IdPelicula,
                Titulo = p.Titulo,
                Descripcion = p.Descripcion,
                FechaLanzamiento = p.FechaLanzamiento,
            });

            return Ok(peliculasDto);
        }

        // GET api/peliculas/cartelera - solo cartelera
        [HttpGet("cartelera")]
        public async Task<IActionResult> GetPeliculasEnCartelera()
        {
            var peliculas = await _peliculaRepository.GetPeliculasEnCarteleraAsync();

            var peliculasDto = peliculas.Select(p => new PeliculaListDto
            {
                IdPelicula = p.IdPelicula,
                Titulo = p.Titulo,
                Descripcion = p.Descripcion,
                FechaLanzamiento = p.FechaLanzamiento,
            });

            return Ok(peliculasDto);
        }

        // GET api/peliculas/{id} - para editar
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPelicula(int id)
        {
            var p = await _peliculaRepository.GetByIdAsync(id);

            if (p == null)
                return NotFound();

            return Ok(new PeliculaListDto
            {
                IdPelicula = p.IdPelicula,
                Titulo = p.Titulo,
                Descripcion = p.Descripcion,
                FechaLanzamiento = p.FechaLanzamiento,
            });
        }

        // POST api/peliculas - crear
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePelicula([FromBody] PeliculaCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            var pelicula = new Pelicula
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaLanzamiento = dto.FechaLanzamiento ?? default,
            };

            await _peliculaRepository.AddAsync(pelicula);

            return CreatedAtAction(nameof(GetPelicula),
                new { id = pelicula.IdPelicula },
                pelicula);
        }

        // PUT api/peliculas/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePelicula(int id, [FromBody] PeliculaCreateDTO dto)
        {
            var pelicula = await _peliculaRepository.GetByIdAsync(id);
            if (pelicula == null)
                return NotFound();

            pelicula.Titulo = dto.Titulo;
            pelicula.Descripcion = dto.Descripcion;
            pelicula.FechaLanzamiento = dto.FechaLanzamiento ?? pelicula.FechaLanzamiento;

            await _peliculaRepository.UpdateAsync(pelicula);

            return NoContent();
        }

        // DELETE api/peliculas/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            var pelicula = await _peliculaRepository.GetByIdAsync(id);
            if (pelicula == null)
                return NotFound();

            await _peliculaRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}

