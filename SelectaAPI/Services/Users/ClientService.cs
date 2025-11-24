using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Integracao.Interfaces;
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
        private readonly HomeRepository _homeRepository;
        private readonly IViaCepIntegracao _viaCepIntegracao;


        public ClientService(IClientRepository clientRepository, IViaCepIntegracao viaCepIntegracao)
        {
            _clientRepository = clientRepository;
            _viaCepIntegracao = viaCepIntegracao;

        }

        public async Task<object> CadastrarEndereco(string cep, int idCliente)
        {
            var enderecoApi = await _viaCepIntegracao.GetDataViaCep(cep);

            if (enderecoApi == null)
                throw new Exception("CEP não encontrado na API ViaCEP.");

            var endereco = new tbEnderecoModel
            {
                Cep = int.Parse(enderecoApi.Cep.Replace("-", "")),
                Logradouro = $"{enderecoApi.Logradouro}, {enderecoApi.Bairro} – {enderecoApi.Localidade},{enderecoApi.Uf}",
                IdCliente = idCliente,
                isPrincipal = enderecoApi.IsPrincipal,
            };

            var enderecoCadastrado = await _clientRepository.CadastrarEndereco(endereco);

            return new
            {
                Mensagem = "Endereço cadastrado com sucesso!",
                Endereco = new
                {
                    enderecoCadastrado.IdEndereco,
                    enderecoCadastrado.Cep,
                    enderecoCadastrado.Logradouro,
                    enderecoCadastrado.IdCliente,
                    enderecoCadastrado.isPrincipal
                }
            };
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
            var verifyIdClient = await _homeRepository.VerificarSeClienteExiste(idCliente);

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
