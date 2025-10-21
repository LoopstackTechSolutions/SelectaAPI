using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface IClientService
    {
        Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO);
        Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO);
    }
}
