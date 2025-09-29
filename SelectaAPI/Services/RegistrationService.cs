using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;
using ZstdSharp.Unsafe;

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
            string hash = PasswordHashHandler.HashPassword(addEmployeeDTO.Senha);

            addEmployeeDTO.Senha = hash;
            var employeeRegister = await _registrationRepository.EmployeeRegister(addEmployeeDTO);
            return employeeRegister;
        }

        public async Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest)
        {
            var addPromotion = await _registrationRepository.PromotionRegister(addPromotionRequest);

            return new AddPromotionResponseDTO
            {
                IdProduto = addPromotion.IdProduto,
                ValorAnterior = addPromotion.ValorAnterior,
                ValorDesconto = addPromotion.ValorDesconto,
                NovoValor = addPromotion.ValorDesconto * addPromotion.ValorAnterior / 100,
                Status = addPromotion.Status,
                Validade = addPromotion.Validade,
            };
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
