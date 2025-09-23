using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;

namespace SelectaAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO)
        {
           var categoryClientRegister = await _registrationRepository.CategoryClientRegister(addCategoryDTO);
            return categoryClientRegister;
        }

        public async Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO)
        {
            string hash = PasswordHashHandler.HashPassword(addClientDTO.Senha);

            addClientDTO.Senha = hash;
            var clientRegister = await _registrationRepository.ClientRegister(addClientDTO);
            return clientRegister;
        }

        public async Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {     
            var employeeRegister = await _registrationRepository.EmployeeRegister(addEmployeeDTO);
            return employeeRegister;
        }
        // TRECHO TESTE
        /*
        public async Task<IEnumerable<GetClientDTO>> GetClient()
        {
            var getClient = await _registrationRepository.GetClient();
            return getClient;
        }
        
        public async Task<IEnumerable<GetEmployeeDTO>> GetEmployee()
        {
            var getEmployee = await _registrationRepository.GetEmployee();
            return getEmployee;
        }
        */
    }
}
