using Refit;
using SelectaAPI.DTOs;

namespace SelectaAPI.Integracao.Refit
{
    public interface IViaCepIntegracaoRefit
    {
        [Get("/ws/{cep}/json/")]
        Task<ApiResponse<AddAdressWithAPI>> GetData(string cep);
    }
}
