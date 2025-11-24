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

        [HttpPost("cadastrar-promocao")]
        public async Task<IActionResult> CadastrarPromocao(AddPromotionRequestDTO addPromotionDTO)
        {
            if (addPromotionDTO == null) return BadRequest("preencha os campos");
            try
            {
                var addPromotion = await _productService.CadastrarPromocao(addPromotionDTO);
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

        [HttpPost("cadastrar-produtos")]
        public async Task<IActionResult> CadastrarProdutos(AddProductDTO addProductDTO)
        {
            try
            {
                var addProduct = await _productService.CadastrarProduto(addProductDTO);
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
        public async Task<IActionResult> AdicionarImagemNoProduto([FromForm] AddImageOfProductDTO addImageDTO)
        {
            try
            {
                var addImage = await _productService.AdicionarImagemNoProduto(addImageDTO);
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
        [HttpGet("buscar-imagem-principal/{idProduto}")]
        public async Task<IActionResult> GetPrincipalImage(int idProduto)
        {
            try
            {
                var imageUrl = await _productService.BuscarImagemPrincipalDoProduto(idProduto);
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
        [HttpGet("todas-imagens/{idProduto}")]
        public async Task<IActionResult> BuscarTodasAsImagensDoProduto(int idProduto)
        {
            try
            {
                var imageUrls = await _productService.BuscarTodasAsImagensDoProduto(idProduto);

                if (!imageUrls.Any())
                    return NotFound($"Nenhuma imagem encontrada para o produto ID {idProduto}");

                return Ok(new
                {
                    IdProduto = idProduto,
                    Imagens = imageUrls
                });
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

        [HttpPut("editar-produto")]
        public async Task<IActionResult> EditarProduto(int idProduto, EditProductDTO editProductDTO)
        {
            try
            {
                if (idProduto == null) return NotFound("ID do produto nulo");
                var editProduct = await _productService.EditarProduto(idProduto, editProductDTO);

                return Ok(editProduct);
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

        [HttpDelete("remover-produto")]
        public async Task<IActionResult> RemoverProduto(int idProduto)
        {
            try
            {
                await _productService.RemoverProduto(idProduto);
                return Ok("Produto deletado com sucesso!");
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
        [HttpDelete("remover-promocao")]
        public async Task<IActionResult> RemoverPromocao(int idPromocao)
        {
            try
            {
                await _productService.RemoverPromocao(idPromocao);
                return Ok("Promoção removida com sucesso!");
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
        [HttpPut("promotion-edit")]
        public async Task<IActionResult> EditarPromocao(EditPromotionRequestDTO editPromotionRequest, int idPromocao)
        {
            try
            {
                await _productService.EditarPromocao  (editPromotionRequest, idPromocao);
                return Ok("Promoção editada com sucesso!");
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

        [HttpGet("pesquisar-produtos")]
        public async Task<IActionResult> PesquisarProdutos(string query)
        {
            try
            {
                var pesquisar = await _productService.PesquisarProdutos(query);
                return Ok(pesquisar);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
