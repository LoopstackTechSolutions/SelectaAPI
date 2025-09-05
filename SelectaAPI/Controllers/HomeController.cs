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
                var highlights = await _homeService.Highlights();
                return Ok(highlights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" erro no servidor{ex.Message}");
            }
        }
        [HttpGet("wish-list")]
        public async Task<IActionResult> WishList()
        {
            try
            {
                if(!User.Identity.IsAuthenticated) return Unauthorized(new { message = "Cliente não está logado"});

                var clientLog = User.Claims.FirstOrDefault(c => c.Type == "id");
                if (clientLog == null) return Unauthorized(new { message = "Id do cliente não encontrado." });

                int id = int.Parse(clientLog.Value);

                var wishList = await _homeService.WishList(id);
                return Ok(wishList);
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
                var notifications = await _homeService.Notifications(id);
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
                var bestSellers  = await _homeService.BestSellers();
                return Ok(bestSellers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }

}

