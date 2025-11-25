using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Services.Users
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISalesPersonRepository _salesPersonRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;

        public SaleService(ISaleRepository saleRepository, ISalesPersonRepository salesPersonRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _salesPersonRepository = salesPersonRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
        }
        public async Task<PedidoResponseDTO> ComprarProduto(PedidoDTO pedidoDTO)
        {
            var verificarCliente = await _clientRepository.ObterClientePorId(pedidoDTO.IdCliente);
            if (verificarCliente == null) throw new ArgumentException("Cliente inexistente!");

            var verificarVendedor = await _salesPersonRepository.ObterProdutoDoVendedor(pedidoDTO.IdProduto);
            if (verificarVendedor == pedidoDTO.IdCliente) throw new ArgumentException("Você não pode comprar o seu próprio produto");

            var preco = await _productRepository.PrecoDoProduto(pedidoDTO.IdProduto);


            var chamarMetodoDoPedido = await _saleRepository.ComprarProduto(pedidoDTO);

            var atualizarSaldoDoVendedor = await _salesPersonRepository.AtualizarSaldoDoVendedor(chamarMetodoDoPedido.Total,pedidoDTO.IdProduto);

            if (chamarMetodoDoPedido == null) throw new Exception("Falha ao gerar pedido");

            return new PedidoResponseDTO
            {
                IdPedido = chamarMetodoDoPedido.IdPedido,
                Total = chamarMetodoDoPedido.Total,
                Mensagem = "Compra realizada com sucesso!"
            };

        }
    }
}
