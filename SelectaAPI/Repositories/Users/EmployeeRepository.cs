using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> VerificarEmailExiste(string email)
        {
            return await _context.funcionarios.AnyAsync(f => f.Email == email);
        }

        public async Task<bool> VerificarCpfExiste(string cpf)
        {
            return await _context.funcionarios.AnyAsync(f => f.Cpf == cpf);
        }

        public async Task<AddEmployeeDTO> CadastrarFuncionario(AddEmployeeDTO addEmployeeDTO)
        {
            var entity = new tbFuncionarioModel
            {
                Nome = addEmployeeDTO.Nome.Trim(),
                Senha = addEmployeeDTO.Senha.Trim(),
                Email = Regex.Replace(addEmployeeDTO.Email.Trim().ToLowerInvariant(), @"\s+", ""),
                Cpf = Regex.Replace(addEmployeeDTO.Cpf, @"\D", ""),
                NivelAcesso = addEmployeeDTO.NivelAcesso.Trim()
            };

            await _context.funcionarios.AddAsync(entity);
            await _context.SaveChangesAsync();

            return addEmployeeDTO;
        }

        public async Task<IEnumerable<tbFuncionarioModel>> ObterListaFuncionarios()
        {
            return await _context.funcionarios
                .Select(f => new tbFuncionarioModel
                {
                    IdFuncionario = f.IdFuncionario,
                    Nome = f.Nome.Trim(),
                    Email = f.Email.Trim(),
                    Cpf = f.Cpf.Trim(),
                    NivelAcesso = f.NivelAcesso
                })
                .ToListAsync();
        }

        public async Task<tbFuncionarioModel> ObterFuncionarioPorId(int idFuncionario)
        {
            return await _context.funcionarios.FindAsync(idFuncionario);
        }

        public async Task RemoverFuncionario(tbFuncionarioModel funcionarioModel)
        {
            _context.funcionarios.Remove(funcionarioModel);
            await _context.SaveChangesAsync();
        }

        public async Task<EditEmployeeDTO> EditarFuncionario(EditEmployeeDTO editEmployeeDTO, int idFuncionario)
        {
            var funcionario = await _context.funcionarios.FirstOrDefaultAsync(f => f.IdFuncionario == idFuncionario);

            if (funcionario == null)
                return null;

            if (!string.IsNullOrWhiteSpace(editEmployeeDTO.Nome))
                funcionario.Nome = editEmployeeDTO.Nome.Trim();

            if (!string.IsNullOrWhiteSpace(editEmployeeDTO.Email))
                funcionario.Email = editEmployeeDTO.Email.Trim().ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(editEmployeeDTO.Cpf))
                funcionario.Cpf = Regex.Replace(editEmployeeDTO.Cpf, @"\D", "");

            if (!string.IsNullOrWhiteSpace(editEmployeeDTO.NivelAcesso))
                funcionario.NivelAcesso = editEmployeeDTO.NivelAcesso;

            if (!string.IsNullOrWhiteSpace(editEmployeeDTO.Senha))
                funcionario.Senha = PasswordHashHandler.HashPassword(editEmployeeDTO.Senha.Trim());

            await _context.SaveChangesAsync();

            return editEmployeeDTO;
        }
    }
}
