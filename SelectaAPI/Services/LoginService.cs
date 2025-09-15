using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repository;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SelectaAPI.Services
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
        public async Task<LoginRequestDTO> ClientLogin(LoginRequestDTO loginRequest)
        {
            var client = await _loginRepository.GetCredentialsOfClient(loginRequest.Email, loginRequest.Senha);
            if (client == null) return null;

            var token = GenerateJwtToken(cliente.IdCliente.ToString(), cliente.Nome); 
            Console.WriteLine($"Email recebido: '{email}'");
            Console.WriteLine($"Senha recebida: '{password}'");

            return new LoginResponseDTO {
        }

        public async Task<LoginRequestDTO> EmployeeLogin(LoginRequestDTO loginRequest)
        {
            Console.WriteLine($"Email recebido: '{email}'");
            Console.WriteLine($"Senha recebida: '{password}'");
            var verification =  await _loginRepository.EmployeeLogin(email, password);
            if (verification == null) throw new UnauthorizedAccessException("erro ao realizar login");
            return verification;
        }
    }
}
