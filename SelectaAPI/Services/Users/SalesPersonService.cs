using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Services.Users
{
    public class SalesPersonService : ISalesPersonService
    {
        private readonly ISalesPersonRepository _salesPersonRepository;
        public SalesPersonService(ISalesPersonRepository salesPersonRepository)
        {
            _salesPersonRepository = salesPersonRepository;
        }
        public async Task<IEnumerable<ResponseMyProductsDTO>> MyProducts(int idVendedor)
        {
            var listaVazia = await _salesPersonRepository.RetornoVazio(idVendedor);
            if (!listaVazia) throw new ArgumentException("Você não possui nenhum produto");

            var callMethod = await _salesPersonRepository.MyProducts(idVendedor);

            return callMethod;  
        }
    }
}
