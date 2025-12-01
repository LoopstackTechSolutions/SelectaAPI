using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Services.Interfaces.UsersInterface;
using System.Security.Claims;

namespace SelectaAPI.Controllers.Users
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        public SaleController(ISaleService saleService)
        {
           _saleService = saleService;
        }
        private int GetClientIdFromToken()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim == null)
                throw new UnauthorizedAccessException("ID do cliente não encontrado no token.");

            return int.Parse(idClaim);
        }


        [HttpPost("comprar-produto")]
        public async Task<IActionResult> ComprarProduto(PedidoDTO pedidoDTO)
        {
            try
            {
                var idCliente = GetClientIdFromToken();
                var chamarMetodoDeCompra = await _saleService.ComprarProduto(pedidoDTO, idCliente);
                return Ok(chamarMetodoDeCompra);
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
