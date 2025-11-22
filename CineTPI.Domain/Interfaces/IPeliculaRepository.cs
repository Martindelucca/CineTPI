using CineTPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.Interfaces
{
    public interface IPeliculaRepository : IRepository<Pelicula>
    {
        Task<IEnumerable<Pelicula>> GetPeliculasEnCarteleraAsync();

    }
}
