using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Repository;
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
        public async Task<LoginResponseDTO> ClientLogin(LoginRequestDTO loginRequest)
        {
            var client = await _loginRepository.GetCredentialsOfClient(loginRequest.Email, loginRequest.Senha);
            if (client == null) return null;

            var token = GenerateJwtToken(client.IdCliente.ToString(), client.Nome);

            return new LoginResponseDTO
            {
                IdCliente = client.IdCliente,
                NomeCliente = client.Nome,
                AccessToken = token,
                ExpiressIn = 60
            };
        }
        private string GenerateJwtToken(string idCliente, string nome)
        {
            var secret = _config["JwtConfig:Key"];

            if (string.IsNullOrEmpty(secret))
                throw new Exception("A chave JWT não foi encontrada. Verifique se 'JwtConfig:Key' está configurado no appsettings.json ou .env.");

            var key = Encoding.UTF8.GetBytes(secret);

            var claims = new List<Claim>
    {
        new Claim("idCliente", idCliente),
        new Claim(ClaimTypes.Name, nome)
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _config["JwtConfig:Issuer"],
                Audience = _config["JwtConfig:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<LoginResponseDTO> EmployeeLogin(LoginRequestDTO loginRequest)
        { 
        var employee = await _loginRepository.GetCredentialsOfEmployee(loginRequest.Email, loginRequest.Senha);
            if (employee == null) return null;

            var token = GenerateJwtToken(employee.IdFuncionario.ToString(), employee.Nome);

            return new LoginResponseDTO
            {
                IdFuncionario = employee.IdFuncionario,
                NomeCliente = employee.Nome,
                AccessToken = token,
                ExpiressIn = 60
            };
        }
    }
}
