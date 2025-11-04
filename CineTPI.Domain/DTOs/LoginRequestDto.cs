using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    // DTO para recibir la solicitud de Login
    public class LoginRequestDto
    {
        public string NroDoc { get; set; }
        public string Password { get; set; } // El password viene en texto plano
    }
}
