using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Services.Interfaces;

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

        // ==================== Session ====================

        private int GetClientIdFromSession()
        {
            int? id = HttpContext.Session.GetInt32("ClientId");

            if (id == null)
                throw new UnauthorizedAccessException("Usuário não está logado ou a sessão expirou.");

            return id.Value;
        }

        // ==================== ENDPOINTS ====================

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
                return StatusCode(500, $"Erro de banco.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro interno do servidor.");
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
                return StatusCode(500, $"Erro de banco.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro interno do servidor.");
            }
        }

        [HttpGet("wish-list")]
        public async Task<IActionResult> WishList()
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var wishList = await _homeService.WishList(idCliente);
                return Ok(wishList);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro interno do servidor.");
            }
        }

        [HttpGet("for-you")]
        public async Task<IActionResult> ForYou()
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var forYou = await _homeService.ForYou(idCliente);
                return Ok(forYou);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, $"Erro de banco.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro interno.");
            }
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> Notifications()
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var notifications = await _homeService.Notifications(idCliente);
                return Ok(notifications);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, $"Erro no servidor.");
            }
        }

        [HttpGet("notifications-unread")]
        public async Task<IActionResult> NotificationsUnread()
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var notifications = await _homeService.NotificationsUnread(idCliente);

                if (notifications == null)
                    return NotFound("Todas as notificações foram lidas.");

                return Ok(notifications);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, $"Erro no servidor.");
            }
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> BestSellers()
        {
            try
            {
                return Ok(await _homeService.BestSellers());
            }
            catch
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [HttpGet("get-products-id")]
        public async Task<IActionResult> GetProductById([FromQuery] int id)
        {
            try
            {
                var product = await _homeService.GetProductByID(id);

                if (product == null)
                    return BadRequest("ID do produto inválido.");

                return Ok(product);
            }
            catch
            {
                return StatusCode(500, "Erro interno.");
            }
        }

        [HttpPost("add-products-wishList")]
        public async Task<IActionResult> AddProductInWishList([FromQuery] int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var result = await _homeService.AddProductInWishList(idProduto, idCliente);

                if (result == null)
                    return BadRequest("Campos insuficientes.");

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [HttpGet("get-products-in-car")]
        public async Task<IActionResult> GetProductsInCartOfClient()
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var cart = await _homeService.GetProductsInCartOfClient(idCliente);
                return Ok(cart);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [HttpGet("verify-type-account")]
        public async Task<IActionResult> GetTypeAccountOfClient()
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var typeAccount = await _homeService.GetTypeAccountOfClient(idCliente);

                return Ok(typeAccount);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [HttpDelete("remove-product-of-cart")]
        public async Task<IActionResult> RemoveProductOfCart([FromQuery] int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                await _homeService.RemoveProductOfCart(idCliente, idProduto);
                return Ok($"Produto removido: {idProduto}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [HttpDelete("remove-product-of-wish-list")]
        public async Task<IActionResult> RemoveProductOfWishList([FromQuery] int idProduto)
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                await _homeService.RemoveProductOfWishList(idCliente, idProduto);
                return Ok($"Produto removido: {idProduto}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }

        [HttpPost("read-notification")]
        public async Task<IActionResult> ReadNotification([FromQuery] int idNotificacao)
        {
            try
            {
                int idCliente = GetClientIdFromSession();
                var result = await _homeService.NotificationsRead(idCliente, idNotificacao);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Erro no servidor.");
            }
        }
    }
}
