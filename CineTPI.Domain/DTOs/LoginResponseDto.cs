using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class LoginResponseDto
    {
        public int CodCliente { get; set; }
        public string NombreCompleto { get; set; }
        public string Token { get; set; } // Aquí irá el JWT (JSON Web Token)
    }
}
