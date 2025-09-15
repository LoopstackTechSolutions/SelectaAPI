using Amazon.Runtime.Internal.Auth;
using MySqlX.XDevAPI;
using SelectaAPI.Models;
using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces
{
    public interface ILoginService
    {
        Task<LoginRequestDTO> ClientLogin (LoginRequestDTO loginRequest);
        Task<LoginRequestDTO> EmployeeLogin(LoginRequestDTO loginRequest);
    }
}
