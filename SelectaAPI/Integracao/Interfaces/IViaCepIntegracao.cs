using SelectaAPI.DTOs;

namespace SelectaAPI.Integracao.Interfaces
{
    public interface IViaCepIntegracao
    {
        Task<AddAdressWithAPI> GetDataViaCep(string cep);
    }
}
