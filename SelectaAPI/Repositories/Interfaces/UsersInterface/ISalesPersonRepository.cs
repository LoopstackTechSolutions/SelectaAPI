using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface ISalesPersonRepository
    {
        Task<IEnumerable<ResponseMyProductsDTO>> MeusProdutos(int idVendedor, int pageNumber = 1, int pageSize = 20);
        Task<bool> RetornoVazio(int idVendedor);
        Task<tbVendedorModel> ObterVendedorPorId(int idVendedor);
        Task<int?> ObterProdutoDoVendedor(int idProduto);
        Task<bool> AtualizarSaldoDoVendedor(decimal total, int idVendedor);
      //  Task<tbProdutoModel> BuscarClienteLigadoAoProduto(int idProduto);
    }
}
