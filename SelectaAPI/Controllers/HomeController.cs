using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using SelectaAPI.Database;
using SelectaAPI.Models;
using System.Linq;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var produtos = await _context.produtos
                    .Select(p => new
                    {
                        p.IdProduto,
                        p.Nome,
                        Quantidade = p.Quantidade ?? 0,
                        p.PrecoUnitario,
                        p.Condicao,
                        Peso = p.Peso ?? 0,
                        p.Status,
                        IdVendedor = p.IdVendedor ?? 0
                    })
                    .ToListAsync();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var search = await _context.produtos.Where(p => EF.Functions.Like(p.Nome, $"%{name}%")).ToListAsync();
            return Ok(search);
        }

        [HttpGet("highlights")]
        public async Task<IActionResult> Highlights()
        {
            try
            {
                var productsPromotion = await _context.promocoes.Include(pp => pp.Produto).
                    Select(pp => new
                    {
                        pp.Status,
                        pp.ValidaAte,
                        pp.Desconto,
                        pp.Produto.Nome,
                        pp.Produto.PrecoUnitario,
                        pp.Produto.Condicao,
                        pp.Produto.Peso,
                        productStatus = pp.Produto.Status,
                        pp.Produto.Quantidade
                    }).
                    Where(pp => pp.Status == "ativa").
                    OrderBy(pp => pp.Desconto).ToListAsync();
                StatusCode(200, "Sucesso na requisição");
                return Ok(productsPromotion);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro no servidor");
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

