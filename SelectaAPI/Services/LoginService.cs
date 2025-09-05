using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Repository;
using SelectaAPI.DTOs;

namespace SelectaAPI.Services
{
    public class LoginService : ILoginService
    {
        private readonly LoginRepository _loginRepository;

        public LoginService(LoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public async Task<ClientLoginDTO> ClientLogin(string email, string password)
        {
            var verification = await _loginRepository.ClientLogin(password, email);
            if (verification == null) throw new Exception("erro ao realizar login");
            return verification;
        }

        public async Task<EmployeeLoginDTO> EmployeeLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email)) throw new Exception("preencha todas as informações");

            var verification =  await _loginRepository.EmployeeLogin(password, email);
            if (verification == null) throw new UnauthorizedAccessException("erro ao realizar login");
            return verification;
        }
    }
}
