using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using System.ComponentModel.Design;

namespace SelectaAPI.Repositories.Users
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;
        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PedidoResponseDTO> ComprarProduto(PedidoDTO pedidoDTO, int idCliente)
        {
                var produto = await _context.produtos.FirstOrDefaultAsync(p => p.IdProduto == pedidoDTO.IdProduto);

                decimal total = produto.PrecoUnitario * pedidoDTO.Quantidade;
                Console.WriteLine($"Unitário: {produto.PrecoUnitario}");
                Console.WriteLine($"Quantidade: {pedidoDTO.Quantidade}");
                Console.WriteLine($"Total calculado: {produto.PrecoUnitario * pedidoDTO.Quantidade}");
                decimal frete = (decimal)(produto.Peso * 0.01 * pedidoDTO.Quantidade);

                var pedido = new tbPedidoModel
                {
                    IdComprador = idCliente,
                    IdEnderecoEntrega = pedidoDTO.IdEnderecoEntrega,
                    DataPedido = DateTime.Now,
                    Total = total,
                    Frete = frete,
                    StatusPagamento = "pendente"
                };
                _context.pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                var produtoPedido = new tbProduto_PedidoModel
                {
                    IdProduto = pedidoDTO.IdProduto,
                    IdPedido = pedido.IdPedido,
                    Quantidade = pedidoDTO.Quantidade,
                    Valor = produto.PrecoUnitario,
                    Frete = frete,
                    Status = "pago",
                    TipoEntrega = true
                };
                _context.produtosPedidos.Add(produtoPedido);

                produto.Quantidade -= pedidoDTO.Quantidade;

                var pagamento = new tbPagamentoModel
                {
                    IdPedido = pedido.IdPedido,
                    DataPagamento = DateTime.Now,
                    FormaPagamento = pedidoDTO.FormaPagamento,
                };
                await _context.SaveChangesAsync();
                return new PedidoResponseDTO
                {
                    IdPedido = pedido.IdPedido,
                    Total = total
                };  
        }

        public async Task<PedidoResponseDTO> ComprarProdutosDoCarrinho( int idCliente, ComprarCarrinhoRequestDTO comprarCarrinho)
        {
                    var itensCarrinho = await _context.carrinho
                    .Include(c => c.Produto)
                    .Where(c => c.IdCliente == idCliente)
                    .ToListAsync();
            var compra = new tbPedidoModel
                {
                    IdComprador = idCliente,
                    IdEnderecoEntrega = comprarCarrinho.IdEnderecoEntrega,
                    DataPedido = DateTime.Now ,
                    Total = 0,
                };
                await _context.pedidos.AddAsync(compra);
                await _context.SaveChangesAsync();

            decimal totalDosProdutos = 0;
            decimal totalDoFrete = 0;

            foreach (var item in itensCarrinho)
            {
                var produto = item.Produto;

                decimal totalItem = produto.PrecoUnitario * item.Quantidade;
                decimal freteItem = (decimal)(produto.Peso * 0.01 * item.Quantidade);

                totalDosProdutos += totalItem;
                totalDoFrete += freteItem;

                var produtoPedido = new tbProduto_PedidoModel
                {
                    IdProduto = produto.IdProduto,
                    IdPedido = compra.IdPedido,
                    Quantidade = item.Quantidade,
                    Valor = produto.PrecoUnitario,
                    Frete = freteItem,
                    Status = "pago",
                    TipoEntrega = true
                };

                _context.produtosPedidos.Add(produtoPedido);

                produto.Quantidade -= item.Quantidade;
                _context.produtos.Update(produto);
            }

            compra.Total = totalDosProdutos;
            compra.Frete = totalDosProdutos;
            _context.pedidos.Update(compra);
            /*
            var pagamento = new tbPagamentoModel
            {
                IdPedido = compra.IdPedido,
                DataPagamento = DateTime.Now,
                FormaPagamento = comprarCarrinho.FormaDePagamento,
 
            };
            _context.Add(pagamento);
            */

            _context.carrinho.RemoveRange(itensCarrinho);

            await _context.SaveChangesAsync();

            return new PedidoResponseDTO
            {
                IdPedido = compra.IdPedido,
                Total = totalDosProdutos + totalDoFrete,
            };
        }
    }
}
