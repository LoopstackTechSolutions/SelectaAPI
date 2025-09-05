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
        public async Task<ClientLoginDTO> ClientLogin(string email, string password)
        {
            Console.WriteLine($"Email recebido: '{email}'");
            Console.WriteLine($"Senha recebida: '{password}'");

            var verification = await _context.clientes
                .Where(f => f.Email.ToLower().Trim() == email.ToLower().Trim()
                         && f.Senha.Trim() == password.Trim())
                .Select(f => new ClientLoginDTO
                {
                    Email = f.Email,
                    Senha = f.Senha
                })
                .FirstOrDefaultAsync();

            return verification;
        }


        public async Task<EmployeeLoginDTO> EmployeeLogin([FromQuery]string email, string password)
        {
            var verification = await _context.funcionarios
                .Where(f => f.Email.ToLower().Trim() == email.ToLower().Trim()
                         && f.Senha.Trim() == password.Trim())
                .Select(f => new EmployeeLoginDTO
                {
                    Email = f.Email,
                    Senha = f.Senha
                })
                .FirstOrDefaultAsync();

            return verification;
        }
    }
}
