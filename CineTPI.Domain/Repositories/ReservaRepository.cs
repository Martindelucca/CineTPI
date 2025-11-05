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
                    // 2. Obtener la función para saber la hora de inicio
                    var funcion = await _context.Funciones
                                                .AsNoTracking() // No lo vamos a modificar
                                                .FirstOrDefaultAsync(f => f.IdFuncion == reservaDto.IdFuncion);

                    if (funcion == null)
                    {
                        throw new Exception("La función seleccionada no existe.");
                    }


                    var horario = await _context.Horarios.FindAsync(funcion.IdHorario);
                    var fechaHoraInicio = funcion.Fecha.ToDateTime(horario.Horario1);
                    var fechaExpiracion = fechaHoraInicio.AddHours(-2);

                    // 4. Crear la cabecera (dbo.reservas)
                    var nuevaReserva = new Reserva
                    {
                        Fecha = DateOnly.FromDateTime(DateTime.Now), // O DateOnly.FromDateTime(DateTime.Now) si tu columna es 'date'
                        CodCliente = codCliente,
                        IdEstadoReserva = 1, 
                        MontoReserva = 0 
                        //  la BBDD no tiene 'fecha_expiracion'.
                        // La lógica de "vencimiento" tendrá que ser un SP o un Job.
                        // Por ahora, solo la creamos.
                    };

                    _context.Reservas.Add(nuevaReserva);
                    await _context.SaveChangesAsync(); // Guardamos para obtener el nuevo ID

                    // 5. Crear los detalles (dbo.reserva_detalle)
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

                    // 6. Si todo salió bien, confirmamos la transacción
                    await transaction.CommitAsync();

                    return nuevaReserva;
                }
                catch (Exception ex)
                {
                    // 7. Si algo falló (ej: butaca ya reservada), revertimos todo
                    await transaction.RollbackAsync();
                    // (Podríamos loggear el error ex)
                    throw new Exception("No se pudo completar la reserva. Intente de nuevo.");
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

        // (También deberías agregar "stubs" o implementaciones vacías
        // para los otros métodos que exige la interfaz)
        public Task AddAsync(Reserva entity)
        {
            _context.Reservas.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            // ... (lógica de borrado)
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas.AsNoTracking().ToListAsync();
        }

        public Task UpdateAsync(Reserva entity)
        {
            // ... (lógica de update)
            throw new NotImplementedException();
        }
    }
}
