using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class ButacaEstadoDto
    {
        public int IdButaca { get; set; }
        public string Fila { get; set; }
        public int Numero { get; set; }
        public string Estado { get; set; } // "Disponible", "Reservada", "Vendida"
    }
}

