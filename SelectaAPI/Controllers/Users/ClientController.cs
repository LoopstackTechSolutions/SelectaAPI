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

        [HttpPost("registrar-cliente")]
        public async Task<IActionResult> RegistrarCliente(AddClientDTO addClientDTO)
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
                await _clientService.CadastrarCliente(addClientDTO);
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

        [HttpPost("registrar-categoria-cliente")]
        public async Task<IActionResult> RegistrarCategoriaCliente(AddCategory_ClientDTO addCategoryClient)
        {
            try
            {
                await _clientService.CadastrarCategoriaDoCliente(addCategoryClient);
                return Ok("Categoria registrada");
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

        [HttpPut("editar-cliente/{idCliente}")]
        public async Task<IActionResult> EditarCliente(int idCliente, EditClientDTO editClientDTO)
        {
            try
            {
                await _clientService.EditarCliente(idCliente, editClientDTO);
                return Ok("Cliente editado com sucesso!");
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

        [HttpDelete("remover-cliente/{idCliente}")]
        public async Task<IActionResult> RemoverCliente(int idCliente)
        {
            try
            {
                await _clientService.RemoverCliente(idCliente);
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
