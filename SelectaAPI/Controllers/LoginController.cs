using Microsoft.AspNetCore.Authorization;
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
        private readonly IJwtService _jwtService;
        public LoginController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }
        [HttpPost("client-login")]
        [AllowAnonymous] 
        public async Task<IActionResult> ClientLogin([FromBody] LoginRequestDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                    return BadRequest("preencha todos os campos");

                var clientLogin = await _jwtService.AuthenticateClient(request);

                if (clientLogin == null)
                    return BadRequest("email ou senha inválidos");

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

                var employeeLogin = await _jwtService.AuthenticateEmployee(request);

                if (employeeLogin == null)
                    return BadRequest("email ou senha inválidos");

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
                Message = "Acesso permitido com token válido ✅"
            });
        }
    }
}
