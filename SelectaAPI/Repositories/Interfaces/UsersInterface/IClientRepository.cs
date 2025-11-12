using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IClientRepository
    {
        Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO);
        Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO);
        Task<EditClientDTO> EditClient(int idCliente,EditClientDTO editClienteDTO);
        Task<bool> EmailVerify(string email);
        Task RemoveClient(tbClienteModel clienteModel);
        Task<tbClienteModel> GetClienteById(int idCliente);
        Task<
    }
}
