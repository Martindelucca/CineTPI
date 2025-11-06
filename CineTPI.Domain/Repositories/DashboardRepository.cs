using CineTPI.Domain;
using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using CineTPI.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; // ¡Importante para FromSqlRaw!
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTPI.Domain.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly CineDBContext _context;

        public DashboardRepository(CineDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReporteReservasDto>> GetReporteReservasPorEstado()
        {
           
            return await _context.Set<ReporteReservasDto>()
                .FromSqlRaw("EXEC pa_reservas_por_estado_mes_actual")
                .ToListAsync();
        }

     

        public async Task<IEnumerable<ReporteReacaudacionDto>> GetReporteRecaudacion(DateTime fechaDesde, DateTime fechaHasta)
        {

            var pFechaDesde = new SqlParameter("@FechaDesde", fechaDesde);
            var pFechaHasta = new SqlParameter("@FechaHasta", fechaHasta);

          
            var sqlQuery = "EXEC SP_Recaudacion_Por_Pelicula @FechaDesde=@FechaDesde, @FechaHasta=@FechaHasta";

            return await _context.Set<ReporteReacaudacionDto>()
                .FromSqlRaw(sqlQuery, pFechaDesde, pFechaHasta)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReporteClientesFrecuentesDto>> GetReporteClientesFrecuentes()
        {
            // (Llamamos al SP sin parámetros, para que use los default)
            return await _context.Set<ReporteClientesFrecuentesDto>()
                .FromSqlRaw("EXEC SP_Clientes_Frecuentes_Promos")
                .ToListAsync();
        }
    }
}