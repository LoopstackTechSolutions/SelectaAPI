using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using SelectaAPI.Database;

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
                Where(pp => pp.Status == "ativa").
                OrderBy(pp => pp.Desconto).ToListAsync();
            return Ok(productsPromotion);
        }

    }
}
