using Amazon.Runtime.Internal.Auth;
using MySqlX.XDevAPI;
using SelectaAPI.Models;
using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface ILoginService
    {
        Task<string> LoginDoCliente (LoginRequestDTO loginRequest);
        Task<string> LoginDoFuncionario(LoginRequestDTO loginRequest);
    }
}
