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
    public class ClienteRepository : IClienteRepository
    {
        private readonly CineDBContext _context;

        public ClienteRepository(CineDBContext context)
        {
            _context = context;
        }
        // --- Métodos Específicos para Login ---

        public async Task<Cliente> GetClienteByDocAndPasswordAsync(string nroDoc, string passwordHash)
        {
            // Busca al cliente por NroDoc y Hash de Password
            // Este método será usado por nuestro servicio de Login
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.NroDoc == nroDoc && c.PasswordHash == passwordHash);
        }

        public async Task<Cliente> GetClienteByDocAsync(string nroDoc)
        {
            // Busca al cliente solo por NroDoc
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.NroDoc == nroDoc);
        }

        // --- Métodos Genéricos (CRUD) ---

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task AddAsync(Cliente entity)
        {
            // Aquí iría la lógica para hashear el password antes de guardarlo
            await _context.Clientes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente entity)
        {
            _context.Clientes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Clientes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
