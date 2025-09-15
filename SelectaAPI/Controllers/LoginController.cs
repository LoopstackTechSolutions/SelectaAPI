using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Services.Interfaces;

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
        public async Task<IActionResult> ClientLogin([FromBody] LoginRequestDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                    return BadRequest("preencha todos os campos");

                var clientLogin = await _loginService.ClientLogin(request.Email, request.Senha);

                if (clientLogin == null) return BadRequest("usuário inválido");

                // Return structured response
                return Ok(new { idCliente = clientLogin.IdCliente, message = "login realizado" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"erro no servidor: {ex.Message}");
            }
        }


        [HttpPost("employee-login")]
        public async Task<IActionResult> EmployeeLogin([FromQuery]string email, string password)
        {
            Console.WriteLine($"Email recebido: '{email}'");
            Console.WriteLine($"Senha recebida: '{password}'");
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email)) return BadRequest("preencha todos os campos");

            var clientLogin = await _loginService.EmployeeLogin(email, password);

            if (clientLogin == null) return StatusCode(400, "usuário inválido");

            return Ok("Login realizado!");
        }
    }
}
