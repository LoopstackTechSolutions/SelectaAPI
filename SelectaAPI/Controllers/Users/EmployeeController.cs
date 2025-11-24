using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
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

       
        [HttpPost("cadastrar-funcionario")]
        public async Task<IActionResult> CadastrarFuncionario(AddEmployeeDTO addEmployeeDTO)
        {
            try
            {
                var employeeRegister = await _employeeService.CadastrarFuncionario(addEmployeeDTO);
                return Ok("Sucesso ao cadastrar funcionário");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro no servidor: {ex.Message}");
            }
        }

        [HttpGet("funcionario/listar")]
        public async Task<IActionResult> ListarFuncionarios()
        {
            try
            {
                var listEmployees = await _employeeService.ListarFuncionarios();
                return Ok(listEmployees);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro no servidor: {ex.Message}");
            }
        }

      
        [HttpDelete("funcionario/remover/{idFuncionario}")]
        public async Task<IActionResult> RemoverFuncionario(int idFuncionario)
        {
            try
            {
                await _employeeService.RemoverFuncionario(idFuncionario);
                return Ok("Funcionário deletado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

      
        [HttpPut("editar-funcionario")]
        public async Task<IActionResult> EditarFuncionario(int idFuncionario, EditEmployeeDTO editEmployee)
        {
            try
            {
                await _employeeService.EditarFuncionario(editEmployee, idFuncionario);
                return Ok("Funcionário editado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
