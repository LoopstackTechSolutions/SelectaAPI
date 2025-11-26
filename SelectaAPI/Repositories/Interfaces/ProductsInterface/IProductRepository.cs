using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc.Formatters;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using System.Runtime.CompilerServices;

namespace SelectaAPI.Repositories.Interfaces.ProductsInterface
{
    public interface IProductRepository
    {
        Task<AddPromotionResponseDTO> CadastrarPromocao(AddPromotionRequestDTO addPromotionRequestDTO);
        Task<AddProductDTO> CadastrarProduto(AddProductDTO addProductDTO);
        Task<AddImageOfProductDTO> AdicionarImagemNoProduto(AddImageOfProductDTO addImageDTO);
        Task<string?> BuscarImagemPrincipalDoProduto(int idProduto);
        Task<IEnumerable<string>> BuscarTodasAsImagensDoProduto(int idProduto);
        Task<EditProductDTO> EditarProduto(int idProduto, EditProductDTO editProductDTO);
        Task<tbProdutoModel> BuscarProdutosPorId(int idProduto);
        Task RemoverProduto(tbProdutoModel produtoModel);
        Task<EditPromotionResponseDTO> EditarPromocao(EditPromotionRequestDTO editPromotionRequest, int idPromocao);
        Task RemoverPromocao(tbPromocaoModel promocaoModel);
        Task<tbPromocaoModel> BuscarPromocaoPorId(int idPromocao);
        Task<IEnumerable<tbProdutoModel>> PesquisarProdutos(string query);
        Task<decimal> PrecoDoProduto(int idProduto);
        Task<bool> VerificarEstoque(int idProduto);
        Task<bool> VerificarStatusDoProduto(int idProduto);
        Task<bool> QuantidadeSelecionada(int idProduto, int quantidade);
    }
}
