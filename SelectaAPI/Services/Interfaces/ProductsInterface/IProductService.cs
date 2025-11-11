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
        Task<IEnumerable<string>> GetAllImagesOfProduct(int idProduto);
        Task<EditProductDTO> EditProduct(int idProduto, EditProductDTO editProductDTO);
        Task RemoveProduct (int idProduct);
        Task<EditPromotionResponseDTO> EditPromotion(EditPromotionRequestDTO editPromotionRequest, int idPromocao);
        Task RemovePromotion(int idPromocao);
    }
}
