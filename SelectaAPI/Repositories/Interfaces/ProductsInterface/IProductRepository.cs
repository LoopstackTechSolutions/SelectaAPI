using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.ProductsInterface
{
    public interface IProductRepository
    {
        Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest);
        Task<AddProductDTO> ProductRegister(AddProductDTO addProductDTO);
        Task<AddImageOfProductDTO> AddImageOfProduct(AddImageOfProductDTO addImageDTO);
        Task<string?> GetPrincipalImage(int idProduto);

    }
}
