using CineTPI.Domain.Interfaces;
using CineTPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CineTPI.Domain.Repositories
{
    public class FuncionRepository : IFuncionRepository
    {
        private readonly CineDBContext _context;
        public FuncionRepository(CineDBContext context)
        {
            _context = context;
        }
        public async Task<Funcion> GetByIdAsync(int id)
        {
            // Usamos "Include" para traer datos relacionados (la Película y la Sala)
            // Esto es muy útil para la API
            return await _context.Funciones
                .Include(f => f.IdPeliculaNavigation) // Asume que EF Power Tools creó la navegación
                .Include(f => f.IdSalaNavigation)   // Asume que EF Power Tools creó la navegación
                .FirstOrDefaultAsync(f => f.IdFuncion == id);
        }

        public async Task<IEnumerable<Funcion>> GetAllAsync()
        {
            return await _context.Funciones.ToListAsync();
        }

        public async Task AddAsync(Funcion entity)
        {
            await _context.Funciones.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Funcion entity)
        {
            _context.Funciones.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Funciones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // --- Métodos Específicos ---

        public async Task<IEnumerable<Funcion>> GetFuncionesPorPeliculaAsync(int idPelicula)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            return await _context.Funciones
                .Where(f => f.IdPelicula == idPelicula && f.Fecha >= hoy)
                .Include(f => f.IdSalaNavigation) // Traemos la info de la sala
                .Include(f => f.IdHorarioNavigation) // Traemos la info del horario
                .OrderBy(f => f.Fecha)
                .ThenBy(f => f.IdHorarioNavigation.Horario1) // Ordena por fecha y luego por hora
                .ToListAsync();
        }

        public Task<IEnumerable<Butaca>> GetButacasOcupadasAsync(int idFuncion)
        {
            // Esta consulta es la más compleja del TPI.
            // Requiere cruzar [tickets] (tabla 'entradas') y [reservas_detalle]
            // La implementaremos cuando construyamos el endpoint de "selección de butacas".
            throw new NotImplementedException("Esta lógica se implementará en la capa de Servicios o aquí más adelante.");
        }
    }
}
