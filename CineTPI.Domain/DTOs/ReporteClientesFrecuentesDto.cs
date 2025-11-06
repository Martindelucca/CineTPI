using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class ReporteClientesFrecuentesDto
    {
        public int CodCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public int PeliculasDistintas { get; set; }
        public int TotalEntradas { get; set; }
        public decimal TotalGastado { get; set; }
        public DateTime? PrimeraCompra { get; set; }
        public DateTime? UltimaCompra { get; set; }
    }
}
