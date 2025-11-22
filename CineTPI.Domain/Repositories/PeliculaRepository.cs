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

        // Obtener película por ID
        public async Task<Pelicula> GetByIdAsync(int id)
        {
            return await _context.Peliculas.FindAsync(id);
        }

        // Obtener todas las películas
        public async Task<IEnumerable<Pelicula>> GetAllAsync()
        {
            return await _context.Peliculas.ToListAsync();
        }

        // Agregar película
        public async Task AddAsync(Pelicula entity)
        {
            await _context.Peliculas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Actualizar película
        public async Task UpdateAsync(Pelicula entity)
        {
            _context.Peliculas.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Eliminar película
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Peliculas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // CARTELERA
        public async Task<IEnumerable<Pelicula>> GetPeliculasEnCarteleraAsync()
        {
            // Por ahora mostramos TODAS LAS PELÍCULAS en cartelera.
            return await _context.Peliculas.ToListAsync();
        }

        public async Task<Cliente> GetClienteByDocAsync(string nroDoc)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.NroDoc == nroDoc);
        }
    }
}
