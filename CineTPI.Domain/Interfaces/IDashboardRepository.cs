using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTPI.Domain.Models;
using CineTPI.Domain.DTOs;

namespace CineTPI.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<ReporteReservasDto>> GetReporteReservasPorEstado();
        Task<IEnumerable<ReporteReacaudacionDto>> GetReporteRecaudacion(DateTime fechaDesde, DateTime fechaHasta);
        Task<IEnumerable<ReporteClientesFrecuentesDto>> GetReporteClientesFrecuentes();

        Task<IEnumerable<FuncionesPorGeneroDto>> GetFuncionesPorGeneroAsync(string genero, DateTime fechaDesde, DateTime fechaHasta);
         
        Task<IEnumerable<PerfilClienteDto>> GetPerfilClienteAsync(int clienteId);
    }
       
}
