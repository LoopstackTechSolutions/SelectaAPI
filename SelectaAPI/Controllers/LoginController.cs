using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using System.Linq;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("client-login")]
        public async Task<IActionResult> ClientLogin(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                StatusCode(400, "email ou senha vazios");
                return BadRequest();
            }

            try
            {
                var verification = await _context.clientes
               .AnyAsync(c => c.Email == email && c.Senha == password);

                if (verification)
                {
                    StatusCode(200, "login realizado");
                    return Ok(verification);
                }

                return StatusCode(400, "erro ao realizar o login");
            }
            catch (Exception ex)
            { 
            return StatusCode(500,ex.Message);
            }
        }
    }
}
