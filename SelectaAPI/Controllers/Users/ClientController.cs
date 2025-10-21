using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Controllers.Users
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
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
                var clientRegister = await _clientService.ClientRegister(addClientDTO);
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

        [HttpPost("category-client-register")]
        public async Task<IActionResult> CategoryClientRegister(AddCategory_ClientDTO addCategoryClient)
        {
            try
            {
                var categoryClientRegister = await _clientService.CategoryClientRegister(addCategoryClient);
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

    }
}
