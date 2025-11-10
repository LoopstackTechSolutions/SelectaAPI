using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface IEmployeeService
    {
        Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO);
        Task<IEnumerable<tbFuncionarioModel>> ListEmployees();
        Task RemoveEmployee(int idFuncionario);
    }
}
