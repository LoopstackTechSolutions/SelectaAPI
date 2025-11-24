using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Services.Users
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<AddEmployeeDTO> CadastrarFuncionario(AddEmployeeDTO addEmployeeDTO)
        {
            if (addEmployeeDTO == null)
                throw new ArgumentException("Preencha os campos.");

            if (string.IsNullOrEmpty(addEmployeeDTO.Nome) ||
                string.IsNullOrEmpty(addEmployeeDTO.Email) ||
                string.IsNullOrEmpty(addEmployeeDTO.Cpf) ||
                string.IsNullOrEmpty(addEmployeeDTO.Senha) ||
                string.IsNullOrEmpty(addEmployeeDTO.NivelAcesso))
                throw new ArgumentException("Necessário preencher todas as informações.");

            if (!addEmployeeDTO.Cpf.All(char.IsDigit) || addEmployeeDTO.Cpf.Length != 11)
                throw new ArgumentException("CPF inválido.");

            var emailJaExiste = await _employeeRepository.VerificarEmailExiste(addEmployeeDTO.Email);
            if (emailJaExiste)
                throw new ArgumentException("E-mail já cadastrado.");

            var cpfJaExiste = await _employeeRepository.VerificarCpfExiste(addEmployeeDTO.Cpf);
            if (cpfJaExiste)
                throw new ArgumentException("CPF já cadastrado.");

            string hash = PasswordHashHandler.HashPassword(addEmployeeDTO.Senha);
            addEmployeeDTO.Senha = hash;

            var registro = await _employeeRepository.CadastrarFuncionario(addEmployeeDTO);
            return registro;
        }

        public async Task<IEnumerable<tbFuncionarioModel>> ListarFuncionarios()
        {
            var lista = await _employeeRepository.ObterListaFuncionarios();
            return lista;
        }

        public async Task RemoverFuncionario(int idFuncionario)
        {
            var funcionario = await _employeeRepository.ObterFuncionarioPorId(idFuncionario);

            if (funcionario == null)
                throw new ArgumentException("Funcionário não encontrado.");

            await _employeeRepository.RemoverFuncionario(funcionario);
        }

        public async Task<EditEmployeeDTO> EditarFuncionario(EditEmployeeDTO editEmployee, int idFuncionario)
        {
            var resultado = await _employeeRepository.EditarFuncionario(editEmployee, idFuncionario);
            return resultado;
        }
    }
}
