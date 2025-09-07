using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectaAPI.Database;
using SelectaAPI.Models;
using SelectaAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;
using Amazon.Runtime.Internal;


namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("client-register")]
        public async Task<IActionResult> ClientRegister(AddClientDTO addClientDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Os dados do cliente não foram enviados.");

            if (string.IsNullOrWhiteSpace(addClientDTO.Nome) ||
                string.IsNullOrWhiteSpace(addClientDTO.Email) ||
                string.IsNullOrWhiteSpace(addClientDTO.Senha))
            {
                return BadRequest("Preencha todos os campos obrigatórios: Nome, Email e Senha.");
            }

            try
            {
                var clientRegister = await _registrationService.ClientRegister(addClientDTO);
                return Ok("Cliente cadastrado com sucesso!");
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
                var employeeRegister = await _registrationService.EmployeeRegister(addEmployeeDTO);
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
        /*
        [HttpPost("address-register")]
        public async Task<IActionResult> AddressRegister()
        {

        }
        */

        [HttpPost("category-client-register")]
        public async Task<IActionResult> CategoryClientRegister(AddCategory_ClientDTO addCategoryClient)
        {
            try
            {
                var categoryClientRegister = await _registrationService.CategoryClientRegister(addCategoryClient);
                return Ok("categoria registrada");
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // TRECHO TESTE
        /*
        [HttpGet("get-client")]
        public async Task<IActionResult> GetClient()
        {
            var getClient = await _registrationService.GetClient();
            return Ok(getClient);   
        }

        [HttpGet("get-employee")]
        public async Task<IActionResult> GetEmployee()
        {
            var getEmployee = await _registrationService.GetEmployee();
            return Ok(getEmployee);
        }
        */
    }
}
