using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class PeliculaVistaDto
    {
        public string PeliculaVista { get; set; }
        public DateTime? Estreno { get; set; }
        public DateTime FechaDeLaFuncion { get; set; }
    }
}
