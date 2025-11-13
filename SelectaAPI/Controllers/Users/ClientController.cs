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

        [HttpPut("edit-client")]
        public async Task<IActionResult> EditClient(int idCliente, EditClientDTO editClientDTO)
        {
            try
            {
                if (idCliente == null) return NotFound("ID do cliente nulo");
                var editClient = await _clientService.EditClient(idCliente, editClientDTO);

                return Ok("ta funcionando");
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

        [HttpDelete("client-remove")]
        public async Task<IActionResult> RemoveClient(int idCliente)
        {
            try
            {
                await _clientService.RemoveClient(idCliente);
                return Ok("Cliente deletado com sucesso!");
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
        [HttpPost("tornar-entregador")]
        public async Task<IActionResult> TornarEntregador(AddEntregadorDTO addEntregador)
        {
            try
            {
                await _clientService.TornarEntregador(addEntregador);
                return Ok("Você está apto a realizar entregas, parabéns!");
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

        [HttpPost("cadastrar-endereco")]
        public async Task<IActionResult> CadastrarEndereco([FromQuery] string cep, [FromQuery] int idCliente)
        {
            try
            {
                var resultado = await _clientService.CadastrarEndereco(cep, idCliente);
                return Ok(resultado);
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
