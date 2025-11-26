using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;

namespace SelectaAPI.Repositories.Users
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;
        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PedidoResponseDTO> ComprarProduto(PedidoDTO pedidoDTO)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var produto = await _context.produtos.FirstOrDefaultAsync(p => p.IdProduto == pedidoDTO.IdProduto);

                decimal total = produto.PrecoUnitario * pedidoDTO.Quantidade;
                Console.WriteLine($"Unitário: {produto.PrecoUnitario}");
                Console.WriteLine($"Quantidade: {pedidoDTO.Quantidade}");
                Console.WriteLine($"Total calculado: {produto.PrecoUnitario * pedidoDTO.Quantidade}");
                decimal frete = (decimal)(produto.Peso * 0.01 * pedidoDTO.Quantidade);

                var pedido = new tbPedidoModel
                {
                    IdComprador = pedidoDTO.IdCliente,
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
                    FormaPagamento = pedidoDTO.FormaPagamento
                };
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new PedidoResponseDTO
                {
                    IdPedido = pedido.IdPedido,
                    Total = total
                };  
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
    }
}
