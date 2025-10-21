using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Repository;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Services.Users
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO)
        {
            var categoryClientRegister = await _clientRepository.CategoryClientRegister(addCategoryDTO);
            return categoryClientRegister;
        }
    
        public async Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO)
        {
            string hash = PasswordHashHandler.HashPassword(addClientDTO.Senha);

            addClientDTO.Senha = hash;
            var clientRegister = await _clientRepository.ClientRegister(addClientDTO);
            return clientRegister;
        }
    }
}
