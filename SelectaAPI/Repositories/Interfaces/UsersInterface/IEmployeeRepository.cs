using SelectaAPI.DTOs;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface IEmployeeRepository
    {
        Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO);
    }
}
