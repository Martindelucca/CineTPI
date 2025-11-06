using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class ReporteReservasDto
    {
        public string EstadoReserva { get; set; }
        public int CantidadReservas { get; set; }
        public decimal TotalRecaudado { get; set; }
    }
}
