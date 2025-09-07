using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO);
        Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO);
        // TRECHO TESTE
        /*
        Task<IEnumerable<GetClientDTO>> GetClient();
        Task<IEnumerable<GetEmployeeDTO>> GetEmployee();
        */
        Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO);
    }
}
