using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Controllers.Users
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("employee-register")]
        public async Task<IActionResult> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {
            if (addEmployeeDTO == null) return BadRequest("preencha os campos");

            if (string.IsNullOrEmpty(addEmployeeDTO.Nome) || string.IsNullOrEmpty(addEmployeeDTO.Email)
                || string.IsNullOrEmpty(addEmployeeDTO.Cpf) || string.IsNullOrEmpty(addEmployeeDTO.Senha)
                || string.IsNullOrEmpty(addEmployeeDTO.NivelAcesso)
                )
            {
                return BadRequest("Necessário preencher todas as informações");
            }

            if (!addEmployeeDTO.Cpf.All(char.IsDigit) || addEmployeeDTO.Cpf.Length < 11 || addEmployeeDTO.Cpf.Length > 11) return BadRequest("CPF inválido");

            try
            {
                var employeeRegister = await _employeeService.EmployeeRegister(addEmployeeDTO);
                return Ok("sucesso ao cadastrar funcionário");
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"erro no servidor {ex.Message}");
            }
        }
        [HttpGet("list-employees")]
        public async Task<IActionResult> ListEmployees()
        {
            try
            {
                var listEmployees = await _employeeService.ListEmployees();

                return Ok(listEmployees); ;
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"erro no servidor {ex.Message}");
            }
        }

    }
}
