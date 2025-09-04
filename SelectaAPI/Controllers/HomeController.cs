using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using SelectaAPI.Database;
using SelectaAPI.Models;
using SelectaAPI.Services;
using System.Linq;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly HomeService _homeService;

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _homeService.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$" erro no servidor{ex.Message}" );
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]string name)
        {
            try
            {
                var search = await _homeService.Search(name);
                return Ok(search);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" erro no servidor{ex.Message}");
            }
        }

        [HttpGet("highlights")]
        public async Task<IActionResult> Highlights()
        {
            try
            {
                var productsPromotion = await _context.promocoes
             .Include(pp => pp.Produto)
             .Select(pp => new
             {
                 IdProduto = pp.Produto.IdProduto,
                 Nome = pp.Produto.Nome,
                 PrecoUnitario = pp.Produto.PrecoUnitario,
                 Condicao = pp.Produto.Condicao,
                 Peso = pp.Produto.Peso ?? 0,
                 Quantidade = pp.Produto.Quantidade ?? 0,
                 Status = pp.Produto.Status,

                 ValidaAte = pp.ValidaAte,
                 Desconto = pp.Desconto
             })
             .OrderBy(pp => pp.Desconto)
             .ToListAsync();

                return Ok(productsPromotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("wish-list")]
        public async Task<IActionResult> WishList([FromQuery] int id)
        {
            try
            {
                var clientUsing = await _context.clientes.Where(c => c.IdCliente == id).FirstOrDefaultAsync();
                if (clientUsing == null)
                {
                    StatusCode(400, "Usuario não informado corretamente");
                    return BadRequest("Usuario não encontrado");
                }
                else
                {
                    var clientList = await _context.listasDesejo.Where(l => l.IdCliente == id).
                     Include(l => l.Produto).
                     Select(l => new
                     {
                         l.Produto.IdProduto,
                         l.Produto.Quantidade,
                         l.Produto.Nome,
                         l.Produto.PrecoUnitario,
                         l.Produto.Condicao,
                         l.Produto.Status,
                         l.Produto.Peso
                     }).
                     ToListAsync();
                    StatusCode(200, "lista de desejo sendo retornada");
                    return Ok(clientList);
                }
            }
            catch (Exception ex)
            {
                StatusCode(500, "erro no servidor");
                return BadRequest();
            }
        }
        [HttpGet("for-you")]
        public async Task<IActionResult> ForYou([FromQuery] int id)
        {
            try
            {
                var clientUsing = await _context.clientes.Where(c => c.IdCliente == id).FirstOrDefaultAsync();
                if (clientUsing == null)
                {
                    StatusCode(400, "Usuario não informado corretamente");
                    return BadRequest("Usuario não encontrado");
                }

                var purchased = await _context.pedidos
                 .Where(p => p.IdComprador == id)
                    .Select(p => p.IdPedido)
                    .ToListAsync();

                if (!purchased.Any())
                    return Ok(new List<object>());

                var purchasedProduct = await _context.produtosPedidos
                    .Where(pp => purchased.Contains(pp.IdPedido))
                    .Select(pp => pp.IdProduto)
                    .Distinct()
                    .ToListAsync();

                if (!purchasedProduct.Any())
                    return Ok(new List<object>());

                var purchasedCategory = await _context.categoriasProdutos
                    .Where(cp => purchasedProduct.Contains(cp.IdProduto))
                    .Select(cp => cp.IdCategoria)
                    .Distinct()
                    .ToListAsync();

                if (!purchasedCategory.Any())
                    return Ok(new List<object>());

                var recommendedProducts = await _context.categoriasProdutos
                    .Where(cp => purchasedCategory.Contains(cp.IdCategoria)
                                 && !purchasedProduct.Contains(cp.IdProduto))
                    .Select(cp => cp.IdProduto)
                    .Distinct()
                    .Take(20)
                    .ToListAsync();

                var recommendationList = await _context.produtos
                    .Where(p => recommendedProducts.Contains(p.IdProduto))
                    .Select(p => new
                    {
                        p.IdProduto,
                        p.Nome,
                        p.PrecoUnitario,
                        p.Peso,
                        p.Condicao,
                        p.Quantidade
                    })
                    .ToListAsync();

                return Ok(recommendationList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("notifications")]
        public async Task<IActionResult> Notifications([FromQuery] int id)
        {
            try
            {
                var clientUsing = await _context.clientes.Where(c => c.IdCliente == id).FirstOrDefaultAsync();
                if (clientUsing == null)
                {
                    StatusCode(400, "Usuario não informado corretamente");
                    return BadRequest("Usuario não encontrado");
                }
                var notifications = await _context.notificacoesClientes.Where(nc => nc.IdCliente == id)
                    .Select(nc => new
                    {
                        nc.DataCriacao,
                        nc.Notificacao.Mensagem,
                        nc.IsLida,
                    }).ToListAsync();
                StatusCode(200, "Notificação sendo mostrada");
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("best-sellers")]
        public async Task<IActionResult> BestSellers()
        {
            try
            {
                var bestSellers = await _context.produtosPedidos.GroupBy(pp => pp.IdProduto)
                   .Select(bs => new
                   {
                       IdProduto = bs.Key,
                       quantitySold = bs.Sum(pp => pp.Quantidade)
                   })
                   .OrderByDescending(bs => bs.quantitySold)
                   .Join(_context.produtos,
                        bs => bs.IdProduto,
                         p => p.IdProduto,
                        (bs, p) => new
                   {
                    p.IdProduto,
                    p.Nome,
                    p.Peso,
                    p.Condicao,
                    p.PrecoUnitario,
                    bs.quantitySold
                   }).
              Take(20)
             .ToListAsync();

                return Ok(bestSellers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }

}

