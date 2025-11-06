using Microsoft.IdentityModel.Tokens;
using SelectaAPI.JWT;
using SelectaAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SelectaAPI.Services
{
    public class TokenService
    {
        public static object GenerateJwtTokenByClient(tbClienteModel cliente)
        {
            var keyBytes = Encoding.UTF8.GetBytes(Key.Secret);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim("clientId", cliente.IdCliente.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new
            {
                Token = tokenString,
            };
        }
    }
}
