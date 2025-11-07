using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTPI.Domain.Models;
using CineTPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineTPI.Domain.Repositories
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly CineDBContext _context;
        public PeliculaRepository(CineDBContext context)
        {
            _context = context;
        }
        // 2. Implementamos los métodos de la interfaz

        public async Task<Pelicula> GetByIdAsync(int id)
        {
            // EF Core traduce esto a: "SELECT * FROM peliculas WHERE id_pelicula = @id"
            return await _context.Peliculas.FindAsync(id);
        }

        public async Task<IEnumerable<Pelicula>> GetAllAsync()
        {
            // EF Core traduce esto a: "SELECT * FROM peliculas"
            return await _context.Peliculas.ToListAsync();
        }

        public async Task AddAsync(Pelicula entity)
        {
            await _context.Peliculas.AddAsync(entity);
            Console.WriteLine("Guardando película en la base de datos...");
            await _context.SaveChangesAsync();
            Console.WriteLine("Película guardada correctamente.");

        }

        public async Task UpdateAsync(Pelicula entity)
        {
            _context.Peliculas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Peliculas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // 3. Implementamos el método específico
        public async Task<IEnumerable<Pelicula>> GetPeliculasEnCarteleraAsync()
        {
            // Ejemplo: Traer películas que tengan funciones programadas a futuro
            // Convertimos el DateTime de hoy a un DateOnly
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            // ¡Esto es el poder de LINQ y EF Core!
            // Escribimos C# y él genera el SQL complejo con JOINs.
            return await _context.Peliculas
                .Where(p => _context.Funciones.Any(f => f.IdPelicula == p.IdPelicula && f.Fecha >= hoy))
                .Distinct() // Trae solo una vez cada película
                .ToListAsync();
        }

        public async Task<Cliente> GetClienteByDocAsync(string nroDoc)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.NroDoc == nroDoc);
        }
    }
}
