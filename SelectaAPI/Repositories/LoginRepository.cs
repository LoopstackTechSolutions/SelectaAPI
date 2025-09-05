using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;
using System.Net.Mail;

namespace SelectaAPI.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        public LoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ClientLoginDTO> ClientLogin([FromQuery] string email, string password)
        {
            var verification = await _context.clientes.Select(c => new ClientLoginDTO
            {
                Email = email,
                Senha = password
            })
           .FirstOrDefaultAsync(c => c.Email == email && c.Senha == password);
            return verification;
        }

        public async Task<EmployeeLoginDTO> EmployeeLogin([FromQuery] string email, string password)
        {
            var verification = await _context.funcionarios.Select(f => new EmployeeLoginDTO
            {
                Email = email,
                Senha = password
            })
           .FirstOrDefaultAsync(c => c.Email == email && c.Senha == password);
            return verification;
        }
    }
}
