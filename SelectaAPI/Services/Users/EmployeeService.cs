using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
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

        public async Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {
            string hash = PasswordHashHandler.HashPassword(addEmployeeDTO.Senha);

            addEmployeeDTO.Senha = hash;
            var employeeRegister = await _employeeRepository.EmployeeRegister(addEmployeeDTO);
            return employeeRegister;
        }

    }
}
