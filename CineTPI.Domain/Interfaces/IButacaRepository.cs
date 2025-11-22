using CineTPI.Domain.DTOs;
using CineTPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.Interfaces
{
    public interface IButacaRepository : IRepository<Butaca>
    {
        //  Traer todas las butacas de una sala específica
        Task<IEnumerable<Butaca>> GetButacasPorSalaAsync(int idSala);

        Task<IEnumerable<ButacaEstadoDto>> GetEstadoButacasPorFuncionAsync(int idFuncion);
    }
}
