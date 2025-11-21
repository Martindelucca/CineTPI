using CineTPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTPI.Domain.DTOs;

namespace CineTPI.Domain.Interfaces
{
    public interface IFuncionRepository : IRepository<Funcion>
    {
        Task<IEnumerable<FuncionDTO>> GetFuncionesPorPeliculaAsync(int idPelicula);
        Task<IEnumerable<Butaca>> GetButacasOcupadasAsync(int idFuncion);
        Task<Funcion> CreateFuncionAsync(FuncionCreateDto dto);
        Task<bool> DeleteFuncionAsync(int idFuncion);
    }
}
