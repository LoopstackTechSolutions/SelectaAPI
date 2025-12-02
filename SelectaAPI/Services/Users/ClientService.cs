using Microsoft.EntityFrameworkCore;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Integracao.Interfaces;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Services.Interfaces.ProductsInterface;
using SelectaAPI.Services.Interfaces.UsersInterface;
using ZstdSharp.Unsafe;

namespace SelectaAPI.Services.Users
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IViaCepIntegracao _viaCepIntegracao;
        private readonly IProductRepository _productRepository;

        public ClientService(IClientRepository clientRepository, IViaCepIntegracao viaCepIntegracao, IProductRepository productRepository)
        {
            _clientRepository = clientRepository;
            _viaCepIntegracao = viaCepIntegracao;
            _productRepository = productRepository;
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

        public async Task<AddCategory_ClientDTO> CadastrarCategoriaDoCliente(AddCategory_ClientDTO addCategoryDTO)
        {
            return await _clientRepository.CadastrarCategoriaDoCliente(addCategoryDTO);
        }

        public async Task<AddClientDTO> CadastrarCliente(AddClientDTO addClientDTO)
        {
            var emailJaExiste = await _clientRepository.VerificarSeEmailExiste(addClientDTO.Email);

            if (emailJaExiste)
                throw new ArgumentException("E-mail já cadastrado.");

            string hash = PasswordHashHandler.HashPassword(addClientDTO.Senha);
            addClientDTO.Senha = hash;

            return await _clientRepository.CadastrarCliente(addClientDTO);
        }

        public async Task<EditClientDTO> EditarCliente(int idCliente, EditClientDTO editClienteDTO)
        {
            var clienteExists = await _clientRepository.ObterClientePorId(idCliente);

            if (clienteExists == null)
                throw new Exception("Cliente não encontrado.");

            return await _clientRepository.EditarCliente(idCliente, editClienteDTO);
        }

        public async Task RemoverCliente(int idCliente)
        {
            var cliente = await _clientRepository.ObterClientePorId(idCliente);

            if (cliente == null)
                throw new ArgumentException("Cliente não encontrado.");

            await _clientRepository.RemoverCliente(cliente);
        }

        public async Task<tbEntregadorModel> TornarEntregador(int idEntregador, AddEntregadorDTO addEntregador)
        {
            if (string.IsNullOrEmpty(addEntregador.Cnh) ||
                !addEntregador.Cnh.All(char.IsDigit) ||
                addEntregador.Cnh.Length != 11)
            {
                throw new ArgumentException("CNH inválida! Preencha o campo corretamente");
            }

            var cliente = await _clientRepository.ObterClientePorId(idEntregador);

            if (cliente == null)
                throw new ArgumentException("ID do cliente não existente");

            return await _clientRepository.TornarSeEntregador(idEntregador, addEntregador);
        }

        public async Task<tbCarrinhoModel> AdicionarProdutoNoCarrinho(int idCliente, AdicionarProdutoNoCarrinhoDTO adicionarDTO)
        {
            var verificarEstoque = await _productRepository.VerificarEstoque(adicionarDTO.IdProduto);
            if (!verificarEstoque) throw new ArgumentException("O produto não possui estoque");

            var verificarCliente = await _clientRepository.ObterClientePorId(idCliente);
            if (verificarCliente == null) throw new ArgumentException("Cliente inexistente");

            var verificarQuantidadeSelecionada = await _productRepository.QuantidadeSelecionada(adicionarDTO.IdProduto, adicionarDTO.Quantidade);
            if (!verificarQuantidadeSelecionada) throw new ArgumentException("Quantidade excedida");

            var adicionarProdutoNoCarrinho = await _clientRepository.AdicionarProdutoNoCarrinho(idCliente,adicionarDTO);
            return adicionarProdutoNoCarrinho;
        }

        public async Task<tbClienteModel> ObterClientePorId(int idCliente)
        {
            var buscarCliente = await _clientRepository.ObterClientePorId(idCliente);
            return buscarCliente;
        }

        public async Task<IEnumerable<tbPedidoModel>> HistoricoDePedidos(int idCliente)
        {
            var verificarSeTemPedidos = await _clientRepository.SemPedidos(idCliente);
            if (!verificarSeTemPedidos) throw new ArgumentException("Você não possui pedidos");

            var historicoDePedidos = await _clientRepository.HistoricoDePedidos(idCliente);
            return historicoDePedidos;
        }

        public async Task<IEnumerable<tbClienteModel>> ListarCliente()
        {
            return await _clientRepository.ListarCliente();
        }
    }
}
