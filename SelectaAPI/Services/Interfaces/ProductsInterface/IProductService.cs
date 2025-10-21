using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.ProductsInterface
{
    public interface IProductService
    {
        Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest);
        Task<AddProductDTO> ProductRegister(AddProductDTO addProduct);
        Task<AddImageOfProductDTO> AddImageOfProduct (AddImageOfProductDTO addImageDTO);
        Task<string?> GetPrincipalImage(int idProduto);
    }
}
