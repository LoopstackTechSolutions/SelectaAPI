using SelectaAPI.DTOs;
using SelectaAPI.Integracao.Interfaces;
using SelectaAPI.Integracao.Refit;
/*
namespace SelectaAPI.Integracao
{
    public class ViaCepIntegracao : IViaCepIntegracao
    {
        private readonly IViaCepIntegracaoRefit _viaCepIntegracaoRefit;

        public ViaCepIntegracao(IViaCepIntegracaoRefit viaCepIntegracao)
        {
            _viaCepIntegracaoRefit = viaCepIntegracao;
        }
        
        public Task<AddAdressWithAPI> GetDataViaCep(string cep)
        {
            var responseData = await _viaCepIntegracaoRefit.GetData(cep);

            if (responseData != null && responseData.IsSuccessStatusCode)
            {
                return responseData.Content();
            }

        }
    }
} 
*/
