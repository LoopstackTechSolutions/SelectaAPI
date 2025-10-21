using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Services.Interfaces.ProductsInterface;

namespace SelectaAPI.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<tbProdutoModel> ProductRegister(IFormFile file, string? prefix)
        {
            return null;
        }

        public async Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest)
        {
            var addPromotion = await _productRepository.PromotionRegister(addPromotionRequest);

            return new AddPromotionResponseDTO
            {
                IdProduto = addPromotion.IdProduto,
                ValorAnterior = addPromotion.ValorAnterior,
                ValorDesconto = addPromotion.ValorDesconto,
                NovoValor = addPromotion.ValorDesconto * addPromotion.ValorAnterior / 100,
                Status = addPromotion.Status,
                Validade = addPromotion.Validade,
            };
        }

    }
}
