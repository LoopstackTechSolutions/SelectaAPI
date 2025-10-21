using SelectaAPI.DTOs;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IClientRepository
    {
        Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO);
        Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO);
    }
}
