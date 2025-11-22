using CineTPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CineTPI.Domain.DTOs;
using System;
using CineTPI.Domain.Repositories;

namespace CineTPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] 
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        // GET: /api/dashboard/reservas-por-estado
        [HttpGet("reservas-por-estado")]
        public async Task<IActionResult> GetReporteReservas()
        {
            var reporte = await _dashboardRepository.GetReporteReservasPorEstado();
            return Ok(reporte);
        }

        [HttpGet("recaudacion-por-pelicula")]
        public async Task<IActionResult> GetReporteRecaudacion(
    [FromQuery] DateTime fechaDesde,
    [FromQuery] DateTime fechaHasta)
        {
            var reporte = await _dashboardRepository.GetReporteRecaudacion(fechaDesde, fechaHasta);
            return Ok(reporte);
        }

        [HttpGet("clientes-frecuentes")]
        public async Task<IActionResult> GetReporteClientes()
        {
            var reporte = await _dashboardRepository.GetReporteClientesFrecuentes();
            return Ok(reporte);
        }
        // GET: api/dashboard/funciones-por-genero
        [HttpGet("funciones-por-genero")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFuncionesPorGenero([FromQuery] string genero, [FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            try
            {
                var data = await _dashboardRepository.GetFuncionesPorGeneroAsync(genero, fechaDesde, fechaHasta);
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error en GetFuncionesPorGenero: {ex.Message}");
                return StatusCode(500, "Error al obtener las funciones por género.");
            }
        }

        [HttpGet("perfil-cliente")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPerfilCliente([FromQuery] int clienteId)
        {
            if (clienteId <= 0)
                return BadRequest("El ID de cliente es inválido.");

            try
            {
                var data = (await _dashboardRepository.GetPerfilClienteAsync(clienteId)).ToList();

                if (!data.Any() || data.First().Info == null)
                    return NotFound("Cliente no encontrado o sin actividad registrada.");

                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error en GetPerfilCliente: {ex}");
                return StatusCode(500, "Error al obtener el perfil del cliente.");
            }
        }

    }



}