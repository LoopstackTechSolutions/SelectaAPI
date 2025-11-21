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

        public async Task<LoginResponseDTO?> ClientLogin(LoginRequestDTO loginRequest)
        {
            var client = await _loginRepository.GetCredentialsOfClient(loginRequest.Email);
            if (client == null) return null; 

            if (!PasswordHashHandler.VerifyPassword(loginRequest.Senha, client.Senha))
                return null; 

            var tokenObj = TokenService.GenerateJwtTokenByClient(client);

           
            // Retorna o DTO de login com o token
            return new LoginResponseDTO
            {
                AccessToken = tokenObj.GetType().GetProperty("Token")!.GetValue(tokenObj)?.ToString()
            };
        }

        public async Task<LoginResponseDTO> EmployeeLogin(LoginRequestDTO loginRequest)
        {
            var employee = await _loginRepository.GetCredentialsOfEmployee(loginRequest.Email);
            if (employee == null) return null;


            if (!PasswordHashHandler.VerifyPassword(loginRequest.Senha, employee.Senha))
                return null;


            return new LoginResponseDTO
            {
                IdFuncionario = employee.IdFuncionario,
                ExpiressIn = 60
            };
        }

        private string GenerateJwtToken(string id, string nome)
        {
            var secret = _config["JwtConfig:Key"];
            if (string.IsNullOrEmpty(secret))
                throw new Exception("A chave JWT não foi encontrada. Verifique se 'JwtConfig:Key' está configurado.");

            var key = Encoding.UTF8.GetBytes(secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),  
                new Claim(ClaimTypes.Name, nome)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _config["JwtConfig:Issuer"],
                Audience = _config["JwtConfig:Audience"],
                SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:Key"])),
        SecurityAlgorithms.HmacSha256
    )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
