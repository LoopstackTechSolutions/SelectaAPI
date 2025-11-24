using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<ProductInWishListDTO>> ListarProdutosDaListaDeDesejos(int idCliente);
        Task<IEnumerable<tbProdutoModel>> ListarProdutosRecomendados(int idCliente);
        Task<IEnumerable<ProductsWithPromotionDTO>> ListarPromocoesDestaque();
        Task<IEnumerable<NotificationForClientDTO>> ListarNotificacoesDoCliente(int idCliente);
        Task<ICollection<NotificationForClientDTO>> ListarNotificacoesNaoLidasDoCliente(int idCliente);
        Task<IEnumerable<tbProdutoModel>> ListarProdutosMaisVendidos();
        Task<tbProdutoModel> ListarProdutoPorId(int idProduto);
        Task<ProductInWishListDTO> AdicionarProdutoNaListaDeDesejos(int idProduto, int idCliente);
        Task<IEnumerable<GetClientCarDTO>> ListarProdutosDoCarrinho(int idCliente);
        Task<IEnumerable<GetClientByIdDTO>> ListarClientePorId(int idCliente);
        Task<IEnumerable<TypeAccountOfClientDTO>> TipoDeConta(int idCliente);
        Task<IEnumerable<SearchProductsByCategoryDTO>> ListarProdutosPorCategoria(int idCategoria);
        Task<IEnumerable<tbProdutoModel>> ListarTodosOsProdutos();
        Task<IEnumerable<tbPromocaoModel>> ListarPromocoesDoProduto(int idProduto);
        Task<tbCarrinhoModel> RemoverProdutoDoCarrinho(int idCliente, int idProduto);
        Task<tbLista_DesejoModel> RemoverProdutoDaListaDeDesejos(int idCliente, int idProduto);
        Task<tbNotificacao_ClienteModel> MarcarNotificacaoComoLida(int idCliente, int idNotificacao);
    }
}
