using Microsoft.IdentityModel.Tokens;
using SelectaAPI.Autenticacao;
using SelectaAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SelectaAPI.Services
{
    public class TokenService
    {
        public static string GenerateJwtTokenByClient(tbClienteModel cliente)
        {
            var keyBytes = Encoding.UTF8.GetBytes(Key.SecretKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim("clientId", cliente.IdCliente.ToString()),
                    new Claim("clientEmail", cliente.Email.ToString()),
                    new Claim ("clientNome", cliente.Nome.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public static string GenerateJwtTokenByEmployee(tbFuncionarioModel funcionario)
        {
            var keyBytes = Encoding.UTF8.GetBytes(Key.SecretKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim("employeeId", funcionario.IdFuncionario.ToString()),
                    new Claim("employeeEmail", funcionario.Email.ToString()),
                    new Claim ("funcionarioNome", funcionario.Nome.ToString()),
                    new Claim (ClaimTypes.Role, funcionario.NivelAcesso.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
