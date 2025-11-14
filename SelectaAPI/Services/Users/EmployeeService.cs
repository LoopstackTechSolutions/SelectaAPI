using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Repositories.Users;
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

        public async Task<EditEmployeeDTO> EditarFuncionario(EditEmployeeDTO editEmployee, int idFuncionario)
        {
            var chamarMetodo = await _employeeRepository.EditarFuncionario(editEmployee, idFuncionario);
            return chamarMetodo;
        }

        public async Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {
            if (addEmployeeDTO == null) throw new ArgumentException("preencha os campos");

            if (string.IsNullOrEmpty(addEmployeeDTO.Nome) || string.IsNullOrEmpty(addEmployeeDTO.Email)
                || string.IsNullOrEmpty(addEmployeeDTO.Cpf) || string.IsNullOrEmpty(addEmployeeDTO.Senha)
                || string.IsNullOrEmpty(addEmployeeDTO.NivelAcesso)) throw new ArgumentException("Necessário preencher todas as informações");
            
            if (!addEmployeeDTO.Cpf.All(char.IsDigit) || addEmployeeDTO.Cpf.Length < 11 || addEmployeeDTO.Cpf.Length > 11) throw new ArgumentException("CPF inválido");
            var verification = await _employeeRepository.EmailVerify(addEmployeeDTO.Email);
            if(verification) throw new ArgumentException("E-mail já cadastrado.");

            var verificacaoCpf = await _employeeRepository.VerificarCpf(addEmployeeDTO.Cpf);
            if(verificacaoCpf) throw new ArgumentException("CPF já cadastrado.");

            string hash = PasswordHashHandler.HashPassword(addEmployeeDTO.Senha);

            addEmployeeDTO.Senha = hash;

            var employeeRegister = await _employeeRepository.EmployeeRegister(addEmployeeDTO);
            return employeeRegister;
        }

        public async Task<IEnumerable<tbFuncionarioModel>> ListEmployees()
        {
            var callMethod = await _employeeRepository.ListEmployees();

            return callMethod;
        }

        public async Task RemoveEmployee(int idFuncionario)
        {
            var employee = await _employeeRepository.GetEmployeeById(idFuncionario);

            if (employee == null)
                throw new ArgumentException("Funcionário não encontrado.");

            await _employeeRepository.RemoveEmployee(employee);
        }
    }
}
