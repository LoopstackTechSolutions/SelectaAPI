using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface ISalesPersonService
    {
        Task<IEnumerable<ResponseMyProductsDTO>> MeusProdutos(int idVendedor);
    }
}
