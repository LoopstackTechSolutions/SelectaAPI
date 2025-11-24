using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface IClientService
    {
        Task<AddClientDTO> CadastrarCliente(AddClientDTO dadosCliente);
        Task<AddCategory_ClientDTO> CadastrarCategoriaDoCliente(AddCategory_ClientDTO dadosCategoria);
        Task<EditClientDTO> EditarCliente(int idCliente, EditClientDTO dadosEditadosCliente);
        Task RemoverCliente(int idCliente);
        Task<tbEntregadorModel> TornarEntregador(AddEntregadorDTO dadosEntregador);
        Task<object> CadastrarEndereco(string cep, int idCliente);
    }
}
