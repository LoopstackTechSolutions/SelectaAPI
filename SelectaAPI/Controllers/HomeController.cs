using Amazon.S3.Model;
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
                return StatusCode(500, $" erro no servidor{ex.Message}");
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            try
            {
                var search = await _homeService.Search(name);
                return Ok(search);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
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
                var highlights = await _homeService.Highlights();
                return Ok(highlights);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $" erro no servidor{ex.Message}");
            }
        }
        [HttpGet("wish-list")]
        public async Task<IActionResult> WishList([FromQuery] int id)
        {
            try
            {
                var wishList = await _homeService.WishList(id);
                return Ok(wishList);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $" erro no servidor{ex.Message}");
            }
        }
        [HttpGet("for-you")]
        public async Task<IActionResult> ForYou([FromQuery] int id)
        {
            try
            {
                var forYou = await _homeService.ForYou(id);
                return Ok(forYou);
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
        [HttpGet("notifications")]
        public async Task<IActionResult> Notifications([FromQuery] int id)
        {
            try
            {
                var notifications = await _homeService.Notifications(id);
                return Ok(notifications);
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

        [HttpGet("notifications-unread")]
        public async Task<IActionResult> NotificationsUnread([FromQuery] int id)
        {
            try
            {
                var notifications = await _homeService.NotificationsUnread(id);

                if (notifications == null) return NotFound("todas as notificações foram lidas");
                return Ok(notifications);
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
        [HttpGet("best-sellers")]
        public async Task<IActionResult> BestSellers()
        {
            try
            {
                var bestSellers = await _homeService.BestSellers();
                return Ok(bestSellers);
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

        [HttpGet("get-products-id")]
        public async Task<IActionResult> GetProductById([FromQuery] int id)
        {
            try
            {
                var getProductById = await _homeService.GetProductByID(id);

                if (getProductById == null) return BadRequest("id do produto nulo");

                return Ok(getProductById);
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

        [HttpPost("add-products-wishList")]
        public async Task<IActionResult> AddProductInWishList([FromQuery] int id, int idCliente)
        {
            try
            {
                var addProductInWishList = await _homeService.AddProductInWishList(id, idCliente);
                if (addProductInWishList == null) return NotFound("preencha todos os campos");
                return Ok(addProductInWishList);
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

        [HttpGet("get-products-in-car")]
        public async Task<IActionResult> GetProductsInCarOfClient([FromQuery] int idClient)
        {
            try
            {
                var getProductsInCar = await _homeService.GetProductsInCarOfClient(idClient);
                return Ok(getProductsInCar);
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

        [HttpGet("verify-type-account")]
        public async Task<IActionResult> GetTypeAccountOfClient([FromQuery] int idClient)
        {
            try
            {
                var getTypeAccountOfClient = await _homeService.GetTypeAccountOfClient(idClient);
                return Ok(getTypeAccountOfClient);
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

