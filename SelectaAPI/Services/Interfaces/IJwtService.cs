using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces
{
    public interface IJwtService
    {
        Task<LoginResponseDTO?> Authenticate(LoginRequestDTO requestDTO);
    }
}
