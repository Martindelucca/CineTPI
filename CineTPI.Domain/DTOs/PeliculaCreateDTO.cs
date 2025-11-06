using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class PeliculaCreateDTO
    {
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateOnly? FechaLanzamiento { get; set; }

    }
}
