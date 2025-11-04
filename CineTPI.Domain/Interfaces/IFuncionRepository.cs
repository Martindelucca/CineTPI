using CineTPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.Interfaces
{
    public interface IFuncionRepository : IRepository<Funcion>
    {
        Task<IEnumerable<Funcion>> GetFuncionesPorPeliculaAsync(int idPelicula);
        Task<IEnumerable<Butaca>> GetButacasOcupadasAsync(int idFuncion);
    }
}
