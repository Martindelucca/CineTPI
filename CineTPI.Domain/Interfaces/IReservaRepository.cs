using CineTPI.Domain.DTOs;
using CineTPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.Interfaces
{
    public interface IReservaRepository : IRepository<Reserva>
    {
        Task<Reserva> CreateReservaAsync(ReservaCreateDto reservaDto, int codCliente);
    }
}
