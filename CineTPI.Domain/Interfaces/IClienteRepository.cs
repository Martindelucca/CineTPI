using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTPI.Domain.Models;

namespace CineTPI.Domain.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> GetClienteByDocAndPasswordAsync(string nroDoc, string passwordHash);
        Task<Cliente> GetClienteByDocAsync(string nroDoc);
    }
}
