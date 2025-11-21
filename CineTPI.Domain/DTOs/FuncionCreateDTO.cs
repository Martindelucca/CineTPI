using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class FuncionCreateDto
    {
        public int IdPelicula { get; set; }
        public int IdSala { get; set; }
        public DateOnly Fecha { get; set; }
        public int IdHorario { get; set; }
    }
}
