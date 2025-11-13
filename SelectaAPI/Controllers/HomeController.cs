using Amazon.S3.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _homeService.GetAll();
                return Ok(products);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("highlights")]
        public async Task<IActionResult> Highlights()
        {
            try
            {
                var highlights = await _homeService.Highlights();
                return Ok(highlights);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("wish-list")]
        public async Task<IActionResult> WishList()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var wishList = await _homeService.WishList(idCliente);
                return Ok(wishList);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("for-you")]
        public async Task<IActionResult> ForYou()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var forYou = await _homeService.ForYou(idCliente);
                return Ok(forYou);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> Notifications()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var notifications = await _homeService.Notifications(idCliente);
                return Ok(notifications);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("notifications-unread")]
        public async Task<IActionResult> NotificationsUnread()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var notifications = await _homeService.NotificationsUnread(idCliente);

                if (notifications == null) return NotFound("Todas as notificações foram lidas");
                return Ok(notifications);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> BestSellers()
        {
            try
            {
                var bestSellers = await _homeService.BestSellers();
                return Ok(bestSellers);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("get-products-id")]
        public async Task<IActionResult> GetProductById([FromQuery] int id)
        {
            try
            {
                var product = await _homeService.GetProductByID(id);
                if (product == null) return BadRequest("ID do produto nulo");
                return Ok(product);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpPost("add-products-wishList")]
        public async Task<IActionResult> AddProductInWishList([FromQuery] int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var result = await _homeService.AddProductInWishList(idProduto, idCliente);
                if (result == null) return NotFound("Preencha todos os campos");
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("get-products-in-car")]
        public async Task<IActionResult> GetProductsInCartOfClient()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var cart = await _homeService.GetProductsInCartOfClient(idCliente);
                return Ok(cart);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpGet("verify-type-account")]
        public async Task<IActionResult> GetTypeAccountOfClient()
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var typeAccount = await _homeService.GetTypeAccountOfClient(idCliente);
                return Ok(typeAccount);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpDelete("remove-product-of-cart")]
        public async Task<IActionResult> RemoveProductOfCart([FromQuery] int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var result = await _homeService.RemoveProductOfCart(idCliente, idProduto);
                return Ok($"Produto removido: {idProduto}");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpDelete("remove-product-of-wish-list")]
        public async Task<IActionResult> RemoveProductOfWishList([FromQuery] int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var result = await _homeService.RemoveProductOfWishList(idCliente, idProduto);
                return Ok($"Produto removido: {idProduto}");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }

        [HttpPost("read-notification")]
        public async Task<IActionResult> ReadNotification([FromQuery] int idNotificacao)
        {
            try
            {
                int idCliente = GetClientIdFromToken();
                var result = await _homeService.NotificationsRead(idCliente, idNotificacao);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco: erro no tratamento dos dados ou falha na conexão.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro no servidor: erro na inicialização do servidor");
            }
        }
    }
}
