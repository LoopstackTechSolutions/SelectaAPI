using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.ProductsInterface
{
    public interface IProductService
    {
        Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest);
        Task<tbProdutoModel> ProductRegister(IFormFile file, string? prefix);
    }
}
