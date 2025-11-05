using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Para leer el ID del token
using System.Threading.Tasks;
using System;
using CineTPI.Domain.Models;
using CineTPI.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace CineTPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ¡Solo usuarios logueados pueden reservar!
    public class ReservasController : ControllerBase
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservasController(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        // POST: /api/reservas
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] ReservaCreateDto reservaDto)
        {
            // --- ¡Seguridad! Obtenemos el ID del cliente DESDE EL TOKEN ---
            // No confiamos en el ID que el frontend nos pueda mandar.
            var idClienteClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClienteClaim == null)
            {
                return Unauthorized("Token no válido o no contiene ID de usuario.");
            }

            var codCliente = int.Parse(idClienteClaim.Value);



            try
            {
                var nuevaReserva = await _reservaRepository.CreateReservaAsync(reservaDto, codCliente);

                // Devolvemos 201 Created y el objeto creado
                return CreatedAtAction(nameof(GetReserva), new { id = nuevaReserva.IdReserva }, nuevaReserva);
            }
            catch (Exception ex)
            {
                // Si el repositorio lanzó una excepción (ej: butaca ocupada), devolvemos 400
                return BadRequest(ex.Message);
            }
        }

        // GET: /api/reservas/5 (Endpoint de soporte para el 'CreatedAtAction')
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(int id)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id);
            if (reserva == null) return NotFound();
            return Ok(reserva);
        }
    }
}