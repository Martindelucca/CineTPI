using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CineTPI.Domain.Interfaces;
using CineTPI.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace CineTPI.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize] 
    public class ButacasController : ControllerBase
    {
        private readonly IButacaRepository _butacaRepository;

        public ButacasController(IButacaRepository butacaRepository)
        {
            _butacaRepository = butacaRepository;
        }

        // GET: /api/butacas/funcion/12
        [HttpGet("funcion/{idFuncion}")]
        public async Task<IActionResult> GetButacasPorFuncion(int idFuncion)
        {
            var butacas = await _butacaRepository.GetEstadoButacasPorFuncionAsync(idFuncion);
            return Ok(butacas);
        }
    }
}
