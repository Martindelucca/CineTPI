using CineTPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CineTPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // ¡Solo Admins pueden ver el dashboard!
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
            // (Podríamos agregar validaciones de fechas aquí)
            var reporte = await _dashboardRepository.GetReporteRecaudacion(fechaDesde, fechaHasta);
            return Ok(reporte);
        }

        [HttpGet("clientes-frecuentes")]
        public async Task<IActionResult> GetReporteClientes()
        {
            var reporte = await _dashboardRepository.GetReporteClientesFrecuentes();
            return Ok(reporte);
        }
    }
}