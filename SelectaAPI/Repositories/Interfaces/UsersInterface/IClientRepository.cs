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
        Task<tbEntregadorModel> TornarSeEntregador(int idEntregador, AddEntregadorDTO addEntregador);
        Task<bool> VerificarSeEnderecoExiste(int idEndereco);
        Task<tbEnderecoModel> CadastrarEndereco(tbEnderecoModel enderecoModel);
        Task<tbCarrinhoModel> AdicionarProdutoNoCarrinho(int idCliente, AdicionarProdutoNoCarrinhoDTO adicionarDTO);
        Task<IEnumerable<tbPedidoModel>> HistoricoDePedidos(int idCliente);
        Task<tbProduto_PedidoModel> DetalhesDoPedido(int idPedido);
        Task<IEnumerable<tbClienteModel>> ListarCliente();
        Task<bool> SemPedidos(int idCliente);
    }
}
