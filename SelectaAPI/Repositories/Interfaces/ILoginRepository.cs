using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repository.Interfaces
{
    public interface ILoginRepository
    {
        Task<tbClienteModel> GetCredentialsOfClient(string email, string password);
        Task<tbFuncionarioModel> GetCredentialsOfEmployee(string email, string password);
    }
}
