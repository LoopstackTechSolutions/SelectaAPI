using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Autenticacao;
using SelectaAPI.DTOs;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Controllers.Users
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
        [AllowAnonymous] 
        public async Task<IActionResult> ClientLogin([FromBody] LoginRequestDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                    return BadRequest("preencha todos os campos");

                var clientLogin = await _loginService.ClientLogin(request);

                if (clientLogin == null)
                    return BadRequest("email ou senha inválidos");

                HttpContext.Session.SetInt32(SessionKeys.UserId, clientLogin.IdCliente);

                HttpContext.Session.SetString(SessionKeys.UserName, clientLogin.Nome);
                HttpContext.Session.SetString(SessionKeys.UserRole, clientLogin.NivelAcesso);
                return Ok(clientLogin);


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
        [AllowAnonymous]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginRequestDTO request)
        {
            try
            {
                 if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                    return BadRequest("preencha todos os campos");

                var employeeLogin = await _loginService.EmployeeLogin(request);
                if (employeeLogin == null)
                    return BadRequest("email ou senha inválidos");

                HttpContext.Session.SetInt32(SessionKeys.UserId, employeeLogin.IdFuncionario);
                HttpContext.Session.SetString(SessionKeys.UserName, employeeLogin.Nome);
                HttpContext.Session.SetString(SessionKeys.UserRole, employeeLogin.NivelAcesso);
                return Ok(employeeLogin);


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

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "idCliente")?.Value;
            var userName = User.Identity?.Name;

            return Ok(new
            {
                Id = userId,
                Nome = userName,
                Message = "Acesso permitido com token válido"
            });
        }
    }
}
