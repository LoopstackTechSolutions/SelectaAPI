using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface IClientService
    {
        Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO);
        Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO);
        Task<EditClientDTO> EditClient(int idCliente, EditClientDTO editClienteDTO);
        Task RemoveClient(int idCliente);
    }
}
