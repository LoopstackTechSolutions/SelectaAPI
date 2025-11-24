using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface IEmployeeService
    {
        Task<AddEmployeeDTO> CadastrarFuncionario(AddEmployeeDTO addEmployeeDTO);
        Task<IEnumerable<tbFuncionarioModel>> ListarFuncionarios();
        Task RemoverFuncionario(int idFuncionario);
        Task<EditEmployeeDTO> EditarFuncionario(EditEmployeeDTO editEmployee, int idFuncionario);
    }
}
