using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;

namespace SelectaAPI.Repositories.Users
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
             _context = context;
        }
        public async Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {
            var entityEmployee = new tbFuncionarioModel()
            {
                Nome = addEmployeeDTO.Nome.Trim(),
                Senha = addEmployeeDTO.Senha.Trim(),
                Email = addEmployeeDTO.Email.Trim(),
                Cpf = addEmployeeDTO.Cpf.Trim(),
                NivelAcesso = addEmployeeDTO.NivelAcesso.Trim()
            };
            await _context.funcionarios.AddAsync(entityEmployee);
            await _context.SaveChangesAsync();

            return addEmployeeDTO;
        }

        public async Task<IEnumerable<tbFuncionarioModel>> ListEmployees()
        {
            var getAllEmployees = await _context.funcionarios.Select(f => new tbFuncionarioModel
            {
                IdFuncionario = f.IdFuncionario,
                Cpf = f.Cpf.Trim(),
                Email = f.Email.Trim(),
                NivelAcesso = f.NivelAcesso,
                Nome = f.Nome.Trim(),
            }).Take(20).ToListAsync();

            return getAllEmployees;
        }
    }
}
