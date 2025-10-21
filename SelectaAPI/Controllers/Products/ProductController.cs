using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
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
            _productService = productService;
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

        [HttpPost("product-register")]
        public async Task<IActionResult> ProductRegister(AddProductDTO addProductDTO)
        {
            try
            {
                var addProduct = await _productService.ProductRegister(addProductDTO);
                return Ok(addProduct);
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
        [HttpPost("image-product-register")]
        public async Task<IActionResult> ImageProductRegister([FromForm] AddImageOfProductDTO addImageDTO)
        {
            try
            {
                var addImage = await _productService.AddImageOfProduct(addImageDTO);
                return Ok(addImage);
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
        [HttpGet("get-principal-image/{idProduto}")]
        public async Task<IActionResult> GetPrincipalImage(int idProduto)
        {
            try
            {
                var imageUrl = await _productService.GetPrincipalImage(idProduto);
                if (imageUrl == null) return NotFound($"Nenhuma imagem encontrada para o produto ID {idProduto}");
                return Ok(imageUrl);
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
