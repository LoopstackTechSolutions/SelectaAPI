using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Services.Interfaces;
using System.Security.Claims;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        
        private int GetClientIdFromToken()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim == null)
                throw new UnauthorizedAccessException("ID do cliente não encontrado no token.");

            return int.Parse(idClaim);
        }

         [HttpGet("listar-todos-produtos")]
        public async Task<IActionResult> ListarTodosOsProdutos()
        {
            try
            {
                var resultado = await _homeService.ListarTodosOsProdutos();
                return Ok(resultado);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro no servidor: erro na inicialização do servidor");
            }
        }

         [HttpGet("destaques")]
        public async Task<IActionResult> ListarDestaques()
        {
            try
            {
                var resultado = await _homeService.ListarPromocoesDestaque();
                return Ok(resultado);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro de banco.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [Authorize]
        [HttpGet("lista-de-desejos")]
        public async Task<IActionResult>ListaDeDesejos()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado= await _homeService.ListarProdutosDaListaDeDesejos(idCliente);
                return Ok(resultado);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro de banco.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [HttpGet("for-you")]
        public async Task<IActionResult> ForYou()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado = await _homeService.ListarProdutosRecomendados(idCliente);
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
        [HttpGet("notificacoes/listar-notificacoes")]
        public async Task<IActionResult> ListarNotificacoes()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado = await _homeService.ListarNotificacoesDoCliente(idCliente);
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
        [HttpGet("notificacoes/notificacoes-nao-lidas")]
        public async Task<IActionResult> ListarNotificacoesNaoLidas()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado = await _homeService.ListarNotificacoesNaoLidasDoCliente(idCliente);

                if (resultado == null) return NotFound("Todas as notificações foram lidas");

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

        [HttpGet("listar-mais-vendidos")]
        public async Task<IActionResult> MaisVendidos()
        {
            try
            {
                var resultado = await _homeService.ListarProdutosMaisVendidos();
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

        [HttpGet("buscar-produto-id/{idProduto}")]
        public async Task<IActionResult> BuscarProdutoPorId(int idProduto)
        {
            try
            {
                var resultado = await _homeService.ListarProdutoPorId(idProduto);

                if (resultado == null) return BadRequest("ID do produto nulo");
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
        [HttpPost("lista-de-desejos/adicionar/{idProduto}")]
        public async Task<IActionResult> AdicionarProdutoNaListaDeDesejos(int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado = await _homeService.AdicionarProdutoNaListaDeDesejos(idProduto, idCliente);

                if (resultado == null) return NotFound("Preencha todos os campos");

                return Ok("Produto adicionado em sua lista!");
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
        [HttpGet("carrinho/listar-produtos/")]
        public async Task<IActionResult> Carrinho()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado = await _homeService.ListarProdutosDoCarrinho(idCliente);
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
        [HttpGet("buscar-tipo-conta/{idCliente}")]
        public async Task<IActionResult> BuscarTipoDeContaDoCliente(int idCliente)
        {
            try
            {
                var resultado = await _homeService.TipoDeConta(idCliente);
                return Ok($"tipo de conta: {resultado}");
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
        [HttpDelete("carrinho/remover{idProduto}")]
        public async Task<IActionResult> RemoverProdutoDoCarrinho(int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                await _homeService.RemoverProdutoDoCarrinho(idCliente, idProduto);
                return Ok($"Produto removido com sucesso!");
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
        [HttpDelete("lista-de-desejos/remover/{idProduto}")]
        public async Task<IActionResult> RemoverProdutoDaListaDeDesejos(int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                await _homeService.RemoverProdutoDaListaDeDesejos(idCliente, idProduto);
                return Ok($"Produto removido: {idProduto}");
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
        [HttpPost("marcar-notificacao-lida/{idNotificacao}")]
        public async Task<IActionResult> MarcarNotificacaoComoLida(int idNotificacao)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var resultado = await _homeService.MarcarNotificacaoComoLida(idCliente, idNotificacao);
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
