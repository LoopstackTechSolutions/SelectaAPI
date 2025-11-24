using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IClientRepository
    {
        Task<AddClientDTO> CadastrarCliente(AddClientDTO dadosCliente);

        Task<AddCategory_ClientDTO> CadastrarCategoriaDoCliente(AddCategory_ClientDTO dadosCategoria);

        Task<EditClientDTO> EditarCliente(int idCliente, EditClientDTO dadosEditadosCliente);

        Task<bool> VerificarSeEmailExiste(string email);

        Task RemoverCliente(tbClienteModel clienteModel);

        Task<tbClienteModel> ObterClientePorId(int idCliente);

        Task<tbEntregadorModel> TornarSeEntregador(AddEntregadorDTO dadosEntregador);

        Task<bool> VerificarSeEnderecoExiste(int idEndereco);

        Task<tbEnderecoModel> CadastrarEndereco(tbEnderecoModel enderecoModel);
    }
}
