using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using System.Text.RegularExpressions;

namespace SelectaAPI.Repositories.Users
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
             _context = context;
        }

        public async Task<bool> EmailVerify(string email)
        {
            var verification = await _context.funcionarios.AnyAsync(f => f.Email == email);
            return verification;
        }

        public async Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {
            var entityEmployee = new tbFuncionarioModel()
            {
                Nome = addEmployeeDTO.Nome.Trim(),
                Senha = addEmployeeDTO.Senha.Trim(),
                Email = Regex.Replace(addEmployeeDTO.Email.Trim().ToLowerInvariant(), @"\s+", ""),
                Cpf = Regex.Replace(addEmployeeDTO.Cpf, @"\D", ""),
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

        public async Task<tbFuncionarioModel> GetEmployeeById(int idFuncionario)
        {
            return await _context.funcionarios.FindAsync(idFuncionario);
        }

        public async Task RemoveEmployee(tbFuncionarioModel funcionarioModel)
        {
            var removeEmployee = _context.funcionarios.Remove(funcionarioModel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerificarCpf(string cpf)
        {
            var verificacao = await _context.funcionarios.AnyAsync(f => f.Cpf == cpf);
            return verificacao;
        }

        public async Task<EditEmployeeDTO> EditarFuncionario(EditEmployeeDTO editEmployee, int idFuncionario)
        {
            var editarFuncionario = await _context.funcionarios.Where(f => f.IdFuncionario == idFuncionario)
               .FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(editEmployee.Nome))
                editarFuncionario.Nome = editEmployee.Nome.Trim();

            if (!string.IsNullOrWhiteSpace(editEmployee.Email))
                editarFuncionario.Email = editEmployee.Email.Trim().ToLower();

            if (editEmployee.Cpf != null)
                editarFuncionario.Cpf = editEmployee.Cpf.Trim();

            if (!string.IsNullOrWhiteSpace(editEmployee.NivelAcesso))
                editarFuncionario.NivelAcesso = editEmployee.NivelAcesso;

            if (!string.IsNullOrWhiteSpace(editEmployee.Senha))
            {
                string hash = PasswordHashHandler.HashPassword(editEmployee.Senha.Trim());
                editarFuncionario.Senha = hash;
            }
            await _context.SaveChangesAsync();
            return editEmployee;
        }
    }
}
