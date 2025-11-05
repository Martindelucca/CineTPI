using CineTPI.Domain.Interfaces;
using CineTPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTPI.Domain.DTOs;

namespace CineTPI.Domain.Repositories
{
    public class ButacaRepository : IButacaRepository
    {
        private readonly CineDBContext _context;

        public ButacaRepository(CineDBContext context)
        {
            _context = context;
        }

        public async Task<Butaca> GetByIdAsync(int id)
        {
            return await _context.Butacas.FindAsync(id);
        }

        public async Task<IEnumerable<Butaca>> GetAllAsync()
        {
            return await _context.Butacas.ToListAsync();
        }

        public async Task AddAsync(Butaca entity)
        {
            await _context.Butacas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Butaca entity)
        {
            _context.Butacas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Butacas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Butaca>> GetButacasPorSalaAsync(int idSala)
        {
            // Trae todas las butacas (físicas) de una sala dada
            return await _context.Butacas
                .Where(b => b.IdSala == idSala)
                .OrderBy(b => b.Fila)
                .ThenBy(b => b.Numero)
                .ToListAsync();
        }

        public async Task<IEnumerable<ButacaEstadoDto>> GetEstadoButacasPorFuncionAsync(int idFuncion)
        {
            // --- 1. Obtener la Sala de la Función ---
            // (Usamos AsNoTracking() para que sea una consulta de solo lectura, es más rápido)
            var funcion = await _context.Funciones
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(f => f.IdFuncion == idFuncion);

            if (funcion == null)
            {
                return new List<ButacaEstadoDto>(); // Función no existe
            }

            // --- 2. Obtener TODAS las butacas de esa sala ---
            // Esta es nuestra lista "base" de asientos físicos
            var butacasDeLaSala = await _context.Butacas
                                                .AsNoTracking()
                                                .Where(b => b.IdSala == funcion.IdSala)
                                                .OrderBy(b => b.Fila)
                                                .ThenBy(b => b.Numero)
                                                .ToListAsync();

            // --- 3. Obtener IDs de butacas VENDIDAS para esa función ---
            // (Asumimos que tu tabla de "tickets vendidos" se llama 'Entradas')
            var idsButacasVendidas = await _context.Entradas
                                                    .AsNoTracking()
                                                    .Where(e => e.IdFuncion == idFuncion)
                                                    .Select(e => e.IdButaca)
                                                    .ToListAsync(); // ToHashSet es muy rápido para buscar

            // --- 4. Obtener IDs de butacas RESERVADAS (activas) para esa función ---
            int ID_ESTADO_RESERVA_ACTIVA = 1; // Asumimos 1 = "Activa"

            var idsButacasReservadas = await _context.ReservaDetalles // Empezamos por el detalle
                                                    .AsNoTracking()
                                                    .Where(rd => rd.IdFuncion == idFuncion)
                                                    // ¡Hacemos JOIN con la cabecera (Reservas) para filtrar por estado!
                                                    .Join(_context.Reservas,
                                                          detalle => detalle.IdReserva,
                                                          cabecera => cabecera.IdReserva,
                                                          (detalle, cabecera) => new { detalle, cabecera }) // Objeto anónimo
                                                    .Where(x => x.cabecera.IdEstadoReserva == ID_ESTADO_RESERVA_ACTIVA)
                                                    .Select(x => x.detalle.IdButaca)
                                                    .ToListAsync();

            // --- 5. Combinar todo en C# (en memoria) ---
            // Recorremos la lista de butacas físicas y les asignamos el estado
            var resultado = butacasDeLaSala.Select(b => new ButacaEstadoDto
            {
                IdButaca = b.IdButaca,
                Fila = b.Fila,
                Numero = b.Numero,

                // Lógica de estado (¡ahora sí!)
                Estado = idsButacasVendidas.Contains(b.IdButaca) ? "Vendida" :
                         idsButacasReservadas.Contains(b.IdButaca) ? "Reservada" :
                                                                     "Disponible"
            });

            return resultado;
        }
    }
}
