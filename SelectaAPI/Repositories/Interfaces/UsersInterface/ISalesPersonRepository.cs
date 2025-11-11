using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface ISalesPersonRepository
    {
        Task<IEnumerable<ResponseMyProductsDTO>> MyProducts(int idVendedor, int pageNumber = 1, int pageSize = 20);
        Task<bool> RetornoVazio(int idVendedor);
    }
}
