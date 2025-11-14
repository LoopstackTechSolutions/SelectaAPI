using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IEmployeeRepository
    {
        Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO);
        Task<IEnumerable<tbFuncionarioModel>> ListEmployees();
        Task<bool> EmailVerify(string email);
        Task<bool> VerificarCpf(string cpf);
        Task RemoveEmployee(tbFuncionarioModel funcionarioModel);
        Task<tbFuncionarioModel> GetEmployeeById(int idFuncionario);
        Task<EditEmployeeDTO> EditarFuncionario(EditEmployeeDTO editEmployee, int idFuncionario);
    }

}
