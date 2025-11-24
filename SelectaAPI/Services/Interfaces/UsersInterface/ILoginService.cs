using Amazon.Runtime.Internal.Auth;
using MySqlX.XDevAPI;
using SelectaAPI.Models;
using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface ILoginService
    {
        Task<LoginResponseDTO?> LoginDoCliente (LoginRequestDTO loginRequest);
        Task<LoginResponseDTO> LoginDoFuncionario(LoginRequestDTO loginRequest);
    }
}
