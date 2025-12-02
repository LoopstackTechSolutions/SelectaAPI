using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SelectaAPI.Repositories.Users
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<tbEnderecoModel> CadastrarEndereco(tbEnderecoModel enderecoModel)
        {
            _context.enderecos.Add(enderecoModel);
            await _context.SaveChangesAsync();
            return enderecoModel;
        }

        public async Task<AddCategory_ClientDTO> CadastrarCategoriaDoCliente(AddCategory_ClientDTO dadosCategoria)
        {
            var cliente = await _context.clientes.FindAsync(dadosCategoria.IdCliente);
            var categoria = await _context.categorias.FindAsync(dadosCategoria.IdCategoria);

            var existeCategoriaParaCliente = await _context.categoriaClientes
                .AnyAsync(cc => cc.IdCliente == dadosCategoria.IdCliente &&
                                cc.IdCategoria == dadosCategoria.IdCategoria);

            var categoriaCliente = new tbCategoria_Cliente
            {
                IdCategoria = dadosCategoria.IdCategoria,
                IdCliente = dadosCategoria.IdCliente
            };

            _context.categoriaClientes.Add(categoriaCliente);
            await _context.SaveChangesAsync();

            return new AddCategory_ClientDTO
            {
                IdCategoria = categoriaCliente.IdCategoria,
                IdCliente = categoriaCliente.IdCliente
            };
        }

        public async Task<AddClientDTO> CadastrarCliente(AddClientDTO dadosCliente)
        {
            var novoCliente = new tbClienteModel()
            {
                Nome = dadosCliente.Nome.Trim(),
                Email = Regex.Replace(dadosCliente.Email.Trim().ToLowerInvariant(), @"\s+", ""),
                Senha = dadosCliente.Senha.Trim()
            };

            await _context.clientes.AddAsync(novoCliente);
            await _context.SaveChangesAsync();

            return dadosCliente;
        }

        public async Task<EditClientDTO> EditarCliente(int idCliente, EditClientDTO dadosEditadosCliente)
        {
            var cliente = await _context.clientes
                .Where(c => c.IdCliente == idCliente)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrWhiteSpace(dadosEditadosCliente.Nome))
                cliente.Nome = dadosEditadosCliente.Nome.Trim();

            if (!string.IsNullOrWhiteSpace(dadosEditadosCliente.Email))
                cliente.Email = dadosEditadosCliente.Email.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(dadosEditadosCliente.Senha))
            {
                string hash = PasswordHashHandler.HashPassword(dadosEditadosCliente.Senha.Trim());
                cliente.Senha = hash;
            }

            await _context.SaveChangesAsync();
            return dadosEditadosCliente;
        }

        public async Task<bool> VerificarSeEmailExiste(string email)
        {
            return await _context.clientes.AnyAsync(c => c.Email == email);
        }

        public async Task<tbClienteModel> ObterClientePorId(int idCliente)
        {
            return await _context.clientes.FindAsync(idCliente);
        }

        public async Task RemoverCliente(tbClienteModel clienteModel)
        {
            _context.clientes.Remove(clienteModel);
            await _context.SaveChangesAsync();
        }

        public async Task<tbEntregadorModel> TornarSeEntregador(int idEntregador, AddEntregadorDTO addEntregador)
        {
            var novoEntregador = new tbEntregadorModel
            {
                IdEntregador = idEntregador,
                IdEndereco = addEntregador.IdEndereco,
                Cnh = Regex.Replace(addEntregador.Cnh, @"\d{11}", ""),
                Eligibilidade = true
            };

            _context.entregadores.Add(novoEntregador);
            await _context.SaveChangesAsync();

            return novoEntregador;
        }

        public async Task<bool> VerificarSeEnderecoExiste(int idEndereco)
        {
            return await _context.enderecos.AnyAsync(e => e.IdEndereco == idEndereco);
        }

        public async Task<tbCarrinhoModel> AdicionarProdutoNoCarrinho(int idCliente, AdicionarProdutoNoCarrinhoDTO adicionarDTO)
        {
            var adicionarProduto = new tbCarrinhoModel()
            {
                IdProduto = adicionarDTO.IdProduto,
                IdCliente = idCliente,
                Quantidade = adicionarDTO.Quantidade
            };
            _context.carrinho.Add(adicionarProduto);
            await _context.SaveChangesAsync(); 

            return adicionarProduto;
        }

        public async Task<IEnumerable<tbPedidoModel>> HistoricoDePedidos(int idCliente)
        {
            var buscarPedidos = await _context.pedidos.Where(c => c.IdComprador == idCliente)
                .Select(c => new tbPedidoModel
                {
                    IdPedido = c.IdPedido,
                    Frete = c.Frete,
                    DataPedido = c.DataPedido,
                    StatusPagamento = c.StatusPagamento,
                    Total = c.Total
                }).ToListAsync();
            return buscarPedidos;
        }

        public async Task<bool> SemPedidos(int idCliente)
        {
           return await _context.pedidos.AnyAsync(c => c.IdComprador == idCliente);
        }

        public async Task<tbProduto_PedidoModel> DetalhesDoPedido(int idPedido)
        {
            var buscarProdutosDoPedido = await _context.produtosPedidos.FirstOrDefaultAsync(pp => pp.IdPedido == idPedido);
            return buscarProdutosDoPedido;
        }

        public async Task<IEnumerable<tbClienteModel>> ListarCliente()
        {
            return await _context.clientes.ToListAsync();
        }
    }
}
