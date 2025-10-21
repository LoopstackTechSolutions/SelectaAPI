using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Services.Interfaces.ProductsInterface;

namespace SelectaAPI.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFilesUploadAWSService _aws;

        public ProductService(IProductRepository productRepository, IFilesUploadAWSService aws)
        {
            _productRepository = productRepository;
            _aws = aws;
        }

        public async Task<AddImageOfProductDTO> AddImageOfProduct(/*IFormFile file, string? prefix,*/ AddImageOfProductDTO addImageDTO)
        {
            var addImage = await _productRepository.AddImageOfProduct(/*file, prefix,*/ addImageDTO);

            return addImage;
        }

        public async Task<IEnumerable<string>> GetAllImagesOfProduct(int idProduto)
        {
            var s3Keys = await _productRepository.GetAllImagesOfProduct(idProduto);

            if (s3Keys == null || !s3Keys.Any())
                return Enumerable.Empty<string>();

            return await _aws.GetAllImages(s3Keys);
        }

        public async Task<string?> GetPrincipalImage(int idProduto)
        {
            var s3Key = await _productRepository.GetPrincipalImage(idProduto);
            if (s3Key == null) return null;

            return await _aws.GetImage(s3Key);
        }

        public async Task<AddProductDTO> ProductRegister(AddProductDTO addProductDTO)
        {
            var addProduct = await _productRepository.ProductRegister(addProductDTO);

            return addProduct;
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
