using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class PerfilClienteInfoDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public decimal TotalGastadoEntradas { get; set; }
        public decimal TotalGastadoProductos { get; set; }
        public string GeneroFavorito { get; set; }


    }
}
