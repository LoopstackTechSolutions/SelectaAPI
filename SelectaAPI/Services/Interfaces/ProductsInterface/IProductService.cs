using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.ProductsInterface
{
    public interface IProductService
    {
        Task<AddPromotionResponseDTO> CadastrarPromocao(AddPromotionRequestDTO addPromotionRequest);
        Task<AddProductDTO> CadastrarProduto(AddProductDTO addProductDTO);
        Task<AddImageOfProductDTO> AdicionarImagemNoProduto(AddImageOfProductDTO addImageDTO);
        Task<string?> BuscarImagemPrincipalDoProduto(int idProduto);
        Task<IEnumerable<string>> BuscarTodasAsImagensDoProduto(int idProduto);
        Task<EditProductDTO> EditarProduto(int idProduto, EditProductDTO editProductDTO);
        Task RemoverProduto(int idProduto);
        Task<EditPromotionResponseDTO> EditarPromocao(EditPromotionRequestDTO editPromotionRequest, int idPromocao);
        Task RemoverPromocao(int idPromocao);
        Task<IEnumerable<tbProdutoModel>> PesquisarProdutos(string query);
    }
}
