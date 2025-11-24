using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IEmployeeRepository
    {
        Task<AddEmployeeDTO> CadastrarFuncionario(AddEmployeeDTO addEmployeeDTO);
        Task<IEnumerable<tbFuncionarioModel>> ObterListaFuncionarios();
        Task<bool> VerificarEmailExiste(string email);
        Task<bool> VerificarCpfExiste(string cpf);
        Task RemoverFuncionario(tbFuncionarioModel funcionarioModel);
        Task<tbFuncionarioModel> ObterFuncionarioPorId(int idFuncionario);
        Task<EditEmployeeDTO> EditarFuncionario(EditEmployeeDTO editEmployeeDTO, int idFuncionario);
    }
}
