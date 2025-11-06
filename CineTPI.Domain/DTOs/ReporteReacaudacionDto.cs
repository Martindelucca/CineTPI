using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class ReporteReacaudacionDto
    {
        public int IdPelicula { get; set; }
        public string Pelicula { get; set; }
        public int TotalEntradasVendidas { get; set; }
        public decimal TotalRecaudado { get; set; }
        public DateTime? PrimeraFuncion { get; set; }
        public DateTime? UltimaFuncion { get; set; }
    }
}
