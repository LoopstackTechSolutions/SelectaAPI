using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.ProductsInterface
{
    public interface IProductRepository
    {
        Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest);
        Task<tbProdutoModel> ProductRegister(IFormFile file, string? prefix, AddProductDTO addProductDTO);
    }
}
