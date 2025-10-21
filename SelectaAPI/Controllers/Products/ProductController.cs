using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Services.Interfaces.ProductsInterface;

namespace SelectaAPI.Controllers.Products
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService    ;
        }

        [HttpPost("promotion-register")]
        public async Task<IActionResult> PromotionRegister(AddPromotionRequestDTO addPromotionDTO)
        {
            if (addPromotionDTO == null) return BadRequest("preencha os campos");
            try
            {
                var addPromotion = await _productService.PromotionRegister(addPromotionDTO);
                return Ok(addPromotion);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
