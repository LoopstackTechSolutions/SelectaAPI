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
        [HttpPost("login-cliente")]
        [AllowAnonymous] 
        public async Task<IActionResult> ClientLogin([FromBody] LoginRequestDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                    return BadRequest("preencha todos os campos");

                var loginDoCliente = await _loginService.LoginDoCliente(request);

                if (loginDoCliente == null)
                    return BadRequest("email ou senha inválidos");

                HttpContext.Session.SetInt32(SessionKeys.UserId, loginDoCliente.IdCliente);

                HttpContext.Session.SetString(SessionKeys.UserName, loginDoCliente.Nome);
                HttpContext.Session.SetString(SessionKeys.UserRole, loginDoCliente.NivelAcesso);
                HttpContext.Session.SetString(SessionKeys.UserRole, loginDoCliente.Email);

                return Ok(loginDoCliente);
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

        [HttpPost("login-funcionario")]
        [AllowAnonymous]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginRequestDTO request)
        {
            try
            {
                 if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                    return BadRequest("preencha todos os campos");

                var loginDoFuncionario= await _loginService.LoginDoFuncionario(request);
                if (loginDoFuncionario == null)
                    return BadRequest("email ou senha inválidos");

                HttpContext.Session.SetInt32(SessionKeys.UserId, loginDoFuncionario.IdFuncionario);
                HttpContext.Session.SetString(SessionKeys.UserName, loginDoFuncionario.Nome);
                HttpContext.Session.SetString(SessionKeys.UserRole, loginDoFuncionario.NivelAcesso);
                HttpContext.Session.SetString(SessionKeys.UserEmail, loginDoFuncionario.Email);

                return Ok(loginDoFuncionario);
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
    }
}
