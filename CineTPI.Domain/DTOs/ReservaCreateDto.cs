using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.DTOs
{
    public class ReservaCreateDto
    {
        public int IdFuncion { get; set; }
        public List<int> IdButacas { get; set; }
    }
}
