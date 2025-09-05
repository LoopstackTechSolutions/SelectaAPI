using Amazon.Runtime.Internal.Auth;
using MySqlX.XDevAPI;
using SelectaAPI.Models;
using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces
{
    public interface ILoginService
    {
        Task<ClientLoginDTO> ClientLogin(string email, string password);
        Task<EmployeeLoginDTO> EmployeeLogin(string email, string password);
    }
}
