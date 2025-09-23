using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Services.Interfaces;
using System.Linq;

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
            // --------- ENDPOINTS LIVRES ---------
            [AllowAnonymous]
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
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            [AllowAnonymous]
            [HttpGet("highlights")]
            public async Task<IActionResult> Highlights()
            {
                try
                {
                    var highlights = await _homeService.Highlights();
                    return Ok(highlights);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            // --------- ENDPOINTS QUE DEPENDEM DO CLIENTE ---------

            [HttpGet("wish-list")]
            public async Task<IActionResult> WishList()
            {
                try
                {
                    var idCliente = int.Parse(User.FindFirst("idCliente")?.Value!);
                    var wishList = await _homeService.WishList(idCliente);
                    return Ok(wishList);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            [HttpGet("for-you")]
            public async Task<IActionResult> ForYou()
            {
                try
                {
                    var idCliente = int.Parse(User.FindFirst("idCliente")?.Value!);
                    var forYou = await _homeService.ForYou(idCliente);
                    return Ok(forYou);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            [HttpGet("notifications")]
            public async Task<IActionResult> Notifications()
            {
                try
                {
                    var idCliente = int.Parse(User.FindFirst("idCliente")?.Value!);
                    var notifications = await _homeService.Notifications(idCliente);
                    return Ok(notifications);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            [HttpGet("notifications-unread")]
            public async Task<IActionResult> NotificationsUnread()
            {
                try
                {
                    var idCliente = int.Parse(User.FindFirst("idCliente")?.Value!);
                    var notifications = await _homeService.NotificationsUnread(idCliente);

                    if (notifications == null)
                        return NotFound("todas as notificações foram lidas");

                    return Ok(notifications);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            [HttpPost("add-products-wishList")]
            public async Task<IActionResult> AddProductInWishList([FromQuery] int idProduto)
            {
                try
                {
                    var idCliente = int.Parse(User.FindFirst("idCliente")?.Value!);
                    var result = await _homeService.AddProductInWishList(idProduto, idCliente);

                    if (result == null)
                        return NotFound("preencha todos os campos");

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            [HttpGet("get-products-in-car")]
            public async Task<IActionResult> GetProductsInCartOfClient()
            {
                try
                {
                    var idCliente = int.Parse(User.FindFirst("idCliente")?.Value!);
                    var result = await _homeService.GetProductsInCartOfClient(idCliente);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }

            [HttpGet("verify-type-account")]
            public async Task<IActionResult> GetTypeAccountOfClient()
            {
                try
                {
                    var idCliente = int.Parse(User.FindFirst("idCliente")?.Value!);
                    var result = await _homeService.GetTypeAccountOfClient(idCliente);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"erro no servidor {ex.Message}");
                }
            }
       [HttpGet("get-client-by-id")]
        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                var getClientById = await _homeService.GetClientById(id);
                return Ok(getClientById);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"erro no servidor:{ex.Message}");
            }
        }
    }
}

 

