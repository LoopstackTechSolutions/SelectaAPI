using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc.Formatters;
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
        Task<IEnumerable<string>> GetAllImagesOfProduct(int idProduto);
    }
}
