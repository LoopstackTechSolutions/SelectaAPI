using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using System.Net.Mail;

namespace SelectaAPI.Repositories.Users
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        public LoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<tbClienteModel> GetCredentialsOfClient(string email, string password)
        {
            return await _context.clientes.FirstOrDefaultAsync(c => c.Email == email && c.Senha == password);   
        }


        public async Task<tbFuncionarioModel> GetCredentialsOfEmployee(string email, string password)
        {
            return await _context.funcionarios
               .FirstOrDefaultAsync(c => c.Email == email && c.Senha == password);
            
        }
    }
}
