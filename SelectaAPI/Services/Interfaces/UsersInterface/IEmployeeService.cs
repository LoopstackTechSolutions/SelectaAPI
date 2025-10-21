using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface IEmployeeService
    {
        Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO);
    }
}
