using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface ILoginRepository
    {
        Task<tbClienteModel> BuscarCredenciaisDoCliente(string email);
        Task<tbFuncionarioModel> BuscarCredenciaisDoFuncionario(string email);
    }
}
