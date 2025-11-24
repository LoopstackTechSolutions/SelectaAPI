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
        /*
        private int GetClientIdFromToken()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim == null)
                throw new UnauthorizedAccessException("ID do cliente não encontrado no token.");

            return int.Parse(idClaim);
        }
        */
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

        [HttpGet("lista-de-desejos/{idCliente}")]
        public async Task<IActionResult> ListarProdutosDaListaDeDesejos(int idCliente)
        {
            try
            {
                var resultadoado = await _homeService.ListarProdutosDaListaDeDesejos(idCliente);
                return Ok(resultadoado);
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

        [HttpGet("for-you/{idCliente}")]
        public async Task<IActionResult> ListarProdutosParaVoce(int idCliente)
        {
            try
            {
                var resultado = await _homeService.ListarProdutosRecomendados(idCliente);
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


        [HttpGet("notificacoes/listar-notificacoes/{idCliente}")]
        public async Task<IActionResult> ListarNotificacoes(int idCliente)
        {
            try
            {
                var resultado = await _homeService.ListarNotificacoesDoCliente(idCliente);
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


        [HttpGet("notificacoes/notificacoes-nao-lidas/{idCliente}")]
        public async Task<IActionResult> ListarNotificacoesNaoLidas(int idCliente)
        {
            try
            {
                var resultado = await _homeService.ListarNotificacoesNaoLidasDoCliente(idCliente);

                if (resultado == null) return NotFound("Todas as notificações foram lidas");

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

        [HttpGet("listar-mais-vendidos")]
        public async Task<IActionResult> ListarMaisVendidos()
        {
            try
            {
                var resultado = await _homeService.ListarProdutosMaisVendidos();
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

        [HttpGet("buscar-produto-id/{idProduto}")]
        public async Task<IActionResult> BuscarProdutoPorId(int idProduto)
        {
            try
            {
                var resultado = await _homeService.ListarProdutoPorId(idProduto);

                if (resultado == null) return BadRequest("ID do produto nulo");
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

        [HttpPost("lista-de-desejos/adicionar/{idProduto}")]
        public async Task<IActionResult> AdicionarProdutoNaListaDeDesejos(int idCliente,int idProduto)
        {
            try
            {
                var resultado = await _homeService.AdicionarProdutoNaListaDeDesejos(idProduto, idCliente);

                if (resultado == null) return NotFound("Preencha todos os campos");

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


        [HttpGet("carrinho/listar-produtos/{idCliente}")]
        public async Task<IActionResult> ListarProdutosNoCarrinhoDoCliente(int idCliente)
        {
            try
            {
                var resultado = await _homeService.ListarProdutosDoCarrinho(idCliente);
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

      
        [HttpGet("buscar-tipo-conta/{idCliente}")]
        public async Task<IActionResult> BuscarTipoDeContaDoCliente(int idCliente)
        {
            try
            {
                var resultado = await _homeService.TipoDeConta(idCliente);
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

        [HttpDelete("carrinho/remover{idProduto}")]
        public async Task<IActionResult> RemoverProdutoDoCarrinho(int idCliente,int idProduto)
        {
            try
            {
                await _homeService.RemoverProdutoDoCarrinho(idCliente, idProduto);
                return Ok($"Produto removido: {idProduto}");
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


        [HttpDelete("lista-de-desejos/remover/{idProduto}")]
        public async Task<IActionResult> RemoverProdutoDaListaDeDesejos(int idCliente, int idProduto)
        {
            try
            {
                await _homeService.RemoverProdutoDaListaDeDesejos(idCliente, idProduto);
                return Ok($"Produto removido: {idProduto}");
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

    
        [HttpPost("marcar-notificacao-lida/{idNotificacao}")]
        public async Task<IActionResult> MarcarNotificacaoComoLida(int idCliente, int idNotificacao)
        {
            try
            {
                var resultado = await _homeService.MarcarNotificacaoComoLida(idCliente, idNotificacao);
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
    }
}
