using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using SelectaAPI.Database;
using SelectaAPI.Models;

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

        // método que fiz pra testar, pode apagar se quiser -Vini
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
                Console.WriteLine(ex); // Logs the exception in your container
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
            var productsPromotion = await _context.promocoes.Include(pp => pp.Produto).
                Select( pp => new{
                    pp.Status,
                    pp.ValidaAte,
                    pp.Desconto,
                    Nome = pp.Produto.Nome,
                    PrecoUnitario = pp.Produto.PrecoUnitario,
                    Condicao = pp.Produto.Condicao,
                    Peso = pp.Produto.Peso,
                    StatusProduto = pp.Produto.Status,
                    Quantidade = pp.Produto.Quantidade
                }).
                Where(pp => pp.Status == "ativa").
                OrderBy(pp => pp.Desconto).ToListAsync();
            return Ok(productsPromotion);
        }

    }
}
