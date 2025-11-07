using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class FuncionesPorGeneroDto
    {
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Sala { get; set; }
        public string TipoSala { get; set; }
        public string Formato { get; set; }
        public DateTime Fecha { get; set; }
        public string Horario { get; set; }
        public string Director { get; set; }
    }
}
