using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc.Formatters;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using System.Runtime.CompilerServices;

namespace SelectaAPI.Repositories.Interfaces.ProductsInterface
{
    public interface IProductRepository
    {
        Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest);
        Task<AddProductDTO> ProductRegister(AddProductDTO addProductDTO);
        Task<AddImageOfProductDTO> AddImageOfProduct(AddImageOfProductDTO addImageDTO);
        Task<string?> GetPrincipalImage(int idProduto);
        Task<IEnumerable<string>> GetAllImagesOfProduct(int idProduto);
        Task<EditProductDTO> EditProduct(int idProduto, EditProductDTO editProductDTO);
        Task<tbProdutoModel> GetProductById(int idProduto);
        Task RemoveProduct(tbProdutoModel produtoModel);
        Task<EditPromotionResponseDTO> EditPromotion(EditPromotionRequestDTO editPromotionRequest, int idPromocao);
        Task RemovePromotion(tbPromocaoModel promocaoModel);
        Task<tbPromocaoModel> GetPromotionById(int idPromocao);
    }
}
