using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Services.Interfaces.UsersInterface;
using System.Security.Claims;

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

        private int GetClientIdFromToken()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim == null)
                throw new UnauthorizedAccessException("ID do cliente não encontrado no token.");

            return int.Parse(idClaim);
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

        [Authorize]
        [HttpPut("editar-cliente")]
        public async Task<IActionResult> EditarCliente(EditClientDTO editClientDTO)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
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

        [Authorize]
        [HttpDelete("remover-cliente")]
        public async Task<IActionResult> RemoverCliente()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
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

        [Authorize]
        [HttpPost("tornar-entregador")]
        public async Task<IActionResult> TornarEntregador(AddEntregadorDTO addEntregador)
        {
            try
            {
                int idEntregador = GetClientIdFromToken();
                await _clientService.TornarEntregador(idEntregador, addEntregador);
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

        [Authorize]
        [HttpPost("buscar-cliente-id/{idCliente}")]
        public async Task<IActionResult> ObterClientePorId(int idCliente)
        {
            try
            {
               var obterCliente = await _clientService.ObterClientePorId(idCliente);
                return Ok(obterCliente);
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

        [Authorize]
        [HttpPost("cadastrar-endereco")]
        public async Task<IActionResult> CadastrarEndereco(string cep)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
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

        [Authorize]
        [HttpPost("carrinho/adicionar-produto")]
        public async Task<IActionResult> AdicionarProdutoNoCarrinho(AdicionarProdutoNoCarrinhoDTO adicionarDTO)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado = await _clientService.AdicionarProdutoNoCarrinho(idCliente, adicionarDTO);
                return Ok("Produto adicionado  no seu carrinho!");
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
