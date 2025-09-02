using Mysqlx;
using SelectaAPI.DTOs;
using SelectaAPI.Integracao.Interfaces;
using SelectaAPI.Integracao.Refit;

namespace SelectaAPI.Integracao
{
    public class ViaCepIntegracao : IViaCepIntegracao
    {
        private readonly IViaCepIntegracaoRefit _viaCepIntegracaoRefit;

        public ViaCepIntegracao(IViaCepIntegracaoRefit viaCepIntegracao)
        {
            _viaCepIntegracaoRefit = viaCepIntegracao;
        }
        
        public async Task<AddAdressWithAPI> GetDataViaCep(string cep)
        {

            var responseData = await _viaCepIntegracaoRefit.GetData(cep);

            if (responseData == null)
                throw new Exception("Resposta nula da API ViaCEP");

            if (!responseData.IsSuccessStatusCode)
                throw new Exception($"Erro API ViaCEP: {responseData.StatusCode} - {responseData.Error?.Message}");

            return responseData.Content;
        }
    }
} 

