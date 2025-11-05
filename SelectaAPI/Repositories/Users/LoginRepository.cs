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
        public async Task<tbClienteModel> GetCredentialsOfClient(string email)
        {
            return await _context.clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<tbFuncionarioModel> GetCredentialsOfEmployee(string email)
        {
            return await _context.funcionarios
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Email == email);
        }
    }
}
