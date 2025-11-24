using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Services.Interfaces.UsersInterface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SelectaAPI.Services.Users
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IConfiguration _config;

        public LoginService(ILoginRepository loginRepository, IConfiguration config)
        {
            _loginRepository = loginRepository;
            _config = config;
        }

        public async Task<LoginResponseDTO?> LoginDoCliente(LoginRequestDTO loginRequest)
        {
            var cliente = await _loginRepository.BuscarCredenciaisDoCliente(loginRequest.Email);
            if (cliente == null) return null; 

            if (!PasswordHashHandler.VerifyPassword(loginRequest.Senha, cliente.Senha))
                return null; 

           
            // Retorna o DTO de login com o token
            return new LoginResponseDTO
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome,
                NivelAcesso = "CLIENTE"
            };
        }

        public async Task<LoginResponseDTO> LoginDoFuncionario(LoginRequestDTO loginRequest)
        {
            var funcionario = await _loginRepository.BuscarCredenciaisDoFuncionario(loginRequest.Email);
            if (funcionario == null) return null;


            if (!PasswordHashHandler.VerifyPassword(loginRequest.Senha, funcionario.Senha))
                return null;


            return new LoginResponseDTO
            {
                IdFuncionario = funcionario.IdFuncionario,
                Nome = funcionario.Nome,
                NivelAcesso = funcionario.NivelAcesso
            };
        }
    }
}
