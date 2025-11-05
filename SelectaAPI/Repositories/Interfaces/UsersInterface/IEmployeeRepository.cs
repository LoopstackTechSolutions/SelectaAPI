using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IEmployeeRepository
    {
        Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO);
        Task<IEnumerable<tbFuncionarioModel>> ListEmployees(); 
    }
}
