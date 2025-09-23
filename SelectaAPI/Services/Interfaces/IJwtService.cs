using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces
{
    public interface IJwtService
    {
        Task<LoginResponseDTO?> AuthenticateClient(LoginRequestDTO requestDTO);
        Task<LoginResponseDTO?> AuthenticateEmployee(LoginRequestDTO requestDTO);
    }
}
