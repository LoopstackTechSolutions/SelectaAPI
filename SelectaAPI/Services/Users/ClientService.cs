using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly HomeRepository _homeRepository; 

        public ClientService(IClientRepository clientRepository, ApplicationDbContext context)
        {
            _clientRepository = clientRepository;
            _context = context;
        }

        public async Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryDTO)
        {
            var categoryClientRegister = await _clientRepository.CategoryClientRegister(addCategoryDTO);
            return categoryClientRegister;
        }

        public async Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO)
        {
            var verification = await _clientRepository.EmailVerify(addClientDTO.Email);
            if (verification) throw new ArgumentException("E-mail já cadastrado.");

            string hash = PasswordHashHandler.HashPassword(addClientDTO.Senha);

            addClientDTO.Senha = hash;
            var clientRegister = await _clientRepository.ClientRegister(addClientDTO);
            return clientRegister;
        }

        public async Task<EditClientDTO> EditClient(int idCliente, EditClientDTO editClienteDTO)
        {
            var verifyIdClient = await _homeRepository.ClientExists(idCliente);

            if (!verifyIdClient) throw new Exception("ID do cliente não existente");

            var callMethodEdit = await _clientRepository.EditClient(idCliente, editClienteDTO);

            return callMethodEdit;
        }

        public async Task RemoveClient(int idCliente)
        {
            var client = await _clientRepository.GetClienteById(idCliente);

            if (client == null)
                throw new ArgumentException("Cliente não encontrado.");

            await _clientRepository.RemoveClient(client);
        }

        public async Task<tbEntregadorModel> TornarEntregador(AddEntregadorDTO addEntregador)
        {
            if (string.IsNullOrEmpty(addEntregador.Cnh) || !addEntregador.Cnh.All(char.IsDigit) || addEntregador.Cnh.Length < 11 || addEntregador.Cnh.Length > 11) 
                throw new ArgumentException("CNH inválida! preencha o campo corretamente");

            var verificarCliente = await _clientRepository.GetClienteById(addEntregador.IdEntregador);

            if (verificarCliente == null) throw new ArgumentException("ID do cliente não existente");

            var cadastrarEntregador = await _clientRepository.TornarEntregador(addEntregador);

            return cadastrarEntregador;
        }
    }
}
