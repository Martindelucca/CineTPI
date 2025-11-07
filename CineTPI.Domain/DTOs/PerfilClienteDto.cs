using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTPI.Domain.DTOs;

namespace CineTPI.Domain.DTOs
{
    public class PerfilClienteDto
    {
        public PerfilClienteInfoDto Info { get; set; }
        public List<PeliculaVistaDto> PeliculasVistas { get; set; } = new();
    }
}
