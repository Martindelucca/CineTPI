using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    // DTO simple con la información de una butaca
    public class ButacaDto
    {
        public int IdButaca { get; set; }
        public int IdSala { get; set; }
        public string Fila { get; set; }
        public int Numero { get; set; }
        public int IdTipoButaca { get; set; } // Para saber si es VIP, etc.
    }
}
