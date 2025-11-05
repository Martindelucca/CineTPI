using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class FuncionDTO
    {
        public int IdFuncion { get; set; }
        public string Fecha { get; set; }
        public string NombreSala { get; set; }
        public string Horario { get; set; }
    }
}
