using CineTPI.Domain.Interfaces;
using CineTPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTPI.Domain.Repositories
{
    public class ButacaRepository : IButacaRepository
    {
        private readonly CineDBContext _context;

        public ButacaRepository(CineDBContext context)
        {
            _context = context;
        }

        public async Task<Butaca> GetByIdAsync(int id)
        {
            return await _context.Butacas.FindAsync(id);
        }

        public async Task<IEnumerable<Butaca>> GetAllAsync()
        {
            return await _context.Butacas.ToListAsync();
        }

        public async Task AddAsync(Butaca entity)
        {
            await _context.Butacas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Butaca entity)
        {
            _context.Butacas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Butacas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Butaca>> GetButacasPorSalaAsync(int idSala)
        {
            // Trae todas las butacas (físicas) de una sala dada
            return await _context.Butacas
                .Where(b => b.IdSala == idSala)
                .OrderBy(b => b.Fila)
                .ThenBy(b => b.Numero)
                .ToListAsync();
        }
    }
}
