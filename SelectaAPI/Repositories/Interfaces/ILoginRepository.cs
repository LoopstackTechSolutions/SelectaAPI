using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repository.Interfaces
{
    public interface ILoginRepository
    {
        Task<ClientLoginDTO> ClientLogin(string email, string password);
        Task<EmployeeLoginDTO> EmployeeLogin(string email, string password);
    }
}
