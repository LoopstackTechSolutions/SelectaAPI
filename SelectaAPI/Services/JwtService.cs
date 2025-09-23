using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SelectaAPI.Services
{

    public class JwtService : IJwtService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public JwtService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> AuthenticateClient(LoginRequestDTO requestDTO)
        {
            if (string.IsNullOrWhiteSpace(requestDTO.Email) || string.IsNullOrWhiteSpace(requestDTO.Senha))
                return null;

            var clientAccount = await _context.clientes.FirstOrDefaultAsync(c => c.Email == requestDTO.Email);

            if (clientAccount == null || !PasswordHashHandler.VerifyPassword(requestDTO.Senha, clientAccount.Senha))
                return null;

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("idCliente", clientAccount.IdCliente.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, requestDTO.Email)
                }),
                Audience = audience,
                Issuer = issuer,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accesToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseDTO
            {
                AccessToken = accesToken,
                NomeCliente = requestDTO.Email,
                IdCliente = clientAccount.IdCliente,
                ExpiressIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }
        public async Task<LoginResponseDTO> AuthenticateEmployee(LoginRequestDTO requestDTO)
        {
            if (string.IsNullOrWhiteSpace(requestDTO.Email) || string.IsNullOrWhiteSpace(requestDTO.Senha))
                return null;

            var employeeAccount = await _context.funcionarios.FirstOrDefaultAsync(c => c.Email == requestDTO.Email);

            if (employeeAccount == null || !PasswordHashHandler.VerifyPassword(requestDTO.Senha, employeeAccount.Senha))
                return null;

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, requestDTO.Email)
                }),
                Audience = audience,
                Issuer = issuer,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accesToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseDTO
            {
                AccessToken = accesToken,
                NomeFuncionario = requestDTO.Email,
                IdFuncionario = employeeAccount.IdFuncionario,
                ExpiressIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };

        }

    }
}

