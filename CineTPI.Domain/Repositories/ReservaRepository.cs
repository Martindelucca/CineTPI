using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTPI.Domain.Models;
using CineTPI.Domain.DTOs;
using CineTPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CineTPI.Domain;
using System.Threading.Tasks;

namespace CineTPI.Domain.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly CineDBContext _context;

        public ReservaRepository(CineDBContext context)
        {
            _context = context;
        }


        public async Task<Reserva> CreateReservaAsync(ReservaCreateDto reservaDto, int codCliente)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //Obtener la función para saber la hora de inicio
                    var funcion = await _context.Funciones
                                                .AsNoTracking() 
                                                .FirstOrDefaultAsync(f => f.IdFuncion == reservaDto.IdFuncion);

                    if (funcion == null)
                    {
                        throw new Exception("La función seleccionada no existe.");
                    }


                    var horario = await _context.Horarios.FindAsync(funcion.IdHorario);
                    var fechaHoraInicio = funcion.Fecha.ToDateTime(horario.Horario1);
                    var fechaExpiracion = fechaHoraInicio.AddHours(-2);

                    // Crear la cabecera
                    var nuevaReserva = new Reserva
                    {
                        Fecha = DateOnly.FromDateTime(DateTime.Now), 
                        CodCliente = codCliente,
                        IdEstadoReserva = 1, 
                        MontoReserva = 0 
                        //  la BBDD no tiene 'fecha_expiracion'.
                        // La lógica de "vencimiento" tendrá que ser un SP o un Job.
                        // Por ahora, solo la creamos.
                    };

                    _context.Reservas.Add(nuevaReserva);
                    await _context.SaveChangesAsync(); // Guardamos para obtener el nuevo ID

                    // Crear los detalles 
                    foreach (var idButaca in reservaDto.IdButacas)
                    {
                        var detalle = new ReservaDetalle
                        {
                            IdReserva = nuevaReserva.IdReserva, // El ID que acabamos de crear
                            IdFuncion = reservaDto.IdFuncion,
                            IdButaca = idButaca
                        };
                        _context.ReservaDetalles.Add(detalle);
                    }

                    await _context.SaveChangesAsync(); // Guardamos los detalles

                    // onfirmamos la transacción
                    await transaction.CommitAsync();

                    return nuevaReserva;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    var mensaje = ex.InnerException?.Message ?? ex.Message;

                    throw new Exception("ERROR REAL → " + mensaje);
                }


            }
        }

        public async Task<Reserva> GetByIdAsync(int id)
        {
            // Implementación real para que el controlador funcione
            return await _context.Reservas
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(r => r.IdReserva == id);
        }

        public Task AddAsync(Reserva entity)
        {
            _context.Reservas.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        { 
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas.AsNoTracking().ToListAsync();
        }

        public Task UpdateAsync(Reserva entity)
        {
            throw new NotImplementedException();
        }
    }
}
