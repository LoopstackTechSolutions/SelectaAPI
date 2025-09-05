using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Repository;
using SelectaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SelectaAPI.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public async Task<ClientLoginDTO> ClientLogin([FromQuery]string email, string password)
        {
            Console.WriteLine($"Email recebido: '{email}'");
            Console.WriteLine($"Senha recebida: '{password}'");
            var verification = await _loginRepository.ClientLogin(email, password);
            if (verification == null) throw new Exception("erro ao realizar login");
            return verification;
        }

        public async Task<EmployeeLoginDTO> EmployeeLogin([FromQuery] string email, string password)
        {
            Console.WriteLine($"Email recebido: '{email}'");
            Console.WriteLine($"Senha recebida: '{password}'");
            var verification =  await _loginRepository.EmployeeLogin(email, password);
            if (verification == null) throw new UnauthorizedAccessException("erro ao realizar login");
            return verification;
        }
    }
}
