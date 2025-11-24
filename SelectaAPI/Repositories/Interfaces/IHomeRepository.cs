using SelectaAPI.Models;
using SelectaAPI.DTOs;

namespace SelectaAPI.Repository.Interfaces
{
    public interface IHomeRepository
    {
        Task<IEnumerable<ProductInWishListDTO>> ObterListaDeDesejos(int idCliente);
        Task<IEnumerable<tbProdutoModel>> ObterProdutosRecomendados(int idCliente);
        Task<IEnumerable<ProductsWithPromotionDTO>> ObterPromocoesDestaque();
        Task<IEnumerable<NotificationForClientDTO>> ObterNotificacoesDoCliente(int idCliente);
        Task<ICollection<NotificationForClientDTO>> ObterNotificacoesNaoLidasDoCliente(int idCliente);
        Task<IEnumerable<tbProdutoModel>> ObterProdutosMaisVendidos();
        Task<tbProdutoModel>ObterProdutoPorId(int idProduto);
        Task<ProductInWishListDTO> AdicionarProdutoNaListaDeDesejos(int idProduto, int idCliente);
        Task<IEnumerable<GetClientByIdDTO>> ObterClientePorId(int idCliente);
        Task<IEnumerable<GetClientCarDTO>> ObterProdutosDoCarrinho(int idCliente);
        Task<IEnumerable<TypeAccountOfClientDTO>> ObterTipoContaClienteVendedor(int idCliente);
        Task<IEnumerable<TypeAccountOfClientDTO>> ObterTipoContaClienteEntregador(int idCliente);
        Task<IEnumerable<SearchProductsByCategoryDTO>> BuscarProdutosPorCategoria(int idCategoria);
        Task<IEnumerable<tbProdutoModel>> ObterTodosProdutos();
        Task<IEnumerable<tbPromocaoModel>> ObterPromocoesPorProduto(int idProduto);
        Task<tbCarrinhoModel> RemoverProdutoDoCarrinho(int idCliente, int idProduto);
        Task<tbLista_DesejoModel> RemoverProdutoDaListaDeDesejos(int idCliente, int idProduto);
        Task<tbNotificacao_ClienteModel> MarcarNotificacaoComoLida(int idCliente, int idNotificacao);
        Task<bool> VerificarSeClienteExiste(int idCliente);
        Task<bool> VerificarSeProdutoExiste(int idProduto);
        Task<bool> VerificarSePromocaoExiste(int idProduto);
    }
}
