using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class PeliculaSimpleDto
    {
        public int IdPelicula { get; set; }
        public string Titulo { get; set; }

        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }

    }
}
