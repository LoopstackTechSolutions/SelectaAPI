using SelectaAPI.DTOs;

namespace SelectaAPI.Repository.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO);
        Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO);
        Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO);
    }
}
