using CineTPI.Domain;
using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using CineTPI.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; // ¡Importante para FromSqlRaw!
using System.Collections.Generic;
using System.Data;
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

        public async Task<IEnumerable<FuncionesPorGeneroDto>> GetFuncionesPorGeneroAsync(string genero, DateTime fechaDesde, DateTime fechaHasta)
        {
            var query = "EXEC sp_funciones @Genero, @FechaDesde, @FechaHasta";

            var parametros = new[]
            {
        new SqlParameter("@Genero", genero ?? (object)DBNull.Value),
        new SqlParameter("@FechaDesde", fechaDesde),
        new SqlParameter("@FechaHasta", fechaHasta)
             };

            return await _context.Set<FuncionesPorGeneroDto>()
                .FromSqlRaw(query, parametros)
                .ToListAsync();
        }

        public async Task<IEnumerable<PerfilClienteDto>> GetPerfilClienteAsync(int clienteId)
        {
            var lista = new List<PerfilClienteDto>();
            var result = new PerfilClienteDto
            {
                Info = new PerfilClienteInfoDto(),
                PeliculasVistas = new List<PeliculaVistaDto>()
            };

            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "usp_ObtenerPerfilCliente";
            cmd.CommandType = CommandType.StoredProcedure;

            var param = cmd.CreateParameter();
            param.ParameterName = "@ClienteID";
            param.Value = clienteId;
            cmd.Parameters.Add(param);

            await using var reader = await cmd.ExecuteReaderAsync();

            // === PRIMER RESULTSET ===
            if (await reader.ReadAsync())
            {
                result.Info = new PerfilClienteInfoDto
                {
                    Nombre = reader["nombre"]?.ToString(),
                    Apellido = reader["apellido"]?.ToString(),
                    FechaRegistro = reader.IsDBNull(reader.GetOrdinal("fecha_registro"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("fecha_registro")),
                    TotalGastadoEntradas = reader.IsDBNull(reader.GetOrdinal("GastoTotalEntradas"))
                        ? 0
                        : reader.GetDecimal(reader.GetOrdinal("GastoTotalEntradas")),
                    // Este SP no tiene GastoTotalProductos, así que lo inicializamos en 0:
                    TotalGastadoProductos = 0,
                    GeneroFavorito = reader["GeneroFavorito"]?.ToString()
                };
            }

            // === SEGUNDO RESULTSET ===
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.PeliculasVistas.Add(new PeliculaVistaDto
                    {
                        PeliculaVista = reader["PeliculaVista"]?.ToString(),
                        Estreno = reader.IsDBNull(reader.GetOrdinal("Estreno"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("Estreno")),
                        FechaDeLaFuncion = (DateTime)(reader.IsDBNull(reader.GetOrdinal("FechaDeLaFuncion"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("FechaDeLaFuncion")))
                    });
                }
            }

            lista.Add(result);
            return lista;
        }


    }
}