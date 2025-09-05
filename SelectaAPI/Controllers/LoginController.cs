using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.Services.Interfaces;
using System.Linq;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("client-login")]
        public async Task<IActionResult> ClientLogin([FromQuery] string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email)) return StatusCode(400, "preencha todos os campos");

                var clientLogin = await _loginService.ClientLogin(email, password);

                if (clientLogin == null) return StatusCode(400, "usuário inválido");

                return Ok(clientLogin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" erro no servidor{ex.Message}");
            }
        }
        [HttpPost("employee-login")]
        public async Task<IActionResult> EmployeeLogin([FromQuery]string email, string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email)) return StatusCode(400, "preencha todos os campos");

            var clientLogin = await _loginService.EmployeeLogin(email, password);

            if (clientLogin == null) return StatusCode(400, "usuário inválido");

            return Ok(clientLogin);
        }
    }
}
