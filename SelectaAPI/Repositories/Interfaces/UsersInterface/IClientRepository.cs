using SelectaAPI.DTOs;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IClientRepository
    {
        Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO);
        Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO);
        Task<EditClientDTO> EditClient(int idCliente,EditClientDTO editClienteDTO);
        Task<AddClientDTO> EmailVerify(AddClientDTO addClientDTO);
    }
}
