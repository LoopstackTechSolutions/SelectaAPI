using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface ILoginRepository
    {
        Task<tbClienteModel> GetCredentialsOfClient(string email);
        Task<tbFuncionarioModel> GetCredentialsOfEmployee(string email);
    }
}
