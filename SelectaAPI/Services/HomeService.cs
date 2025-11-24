using SelectaAPI.Models;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SelectaAPI.DTOs;
using SelectaAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace SelectaAPI.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;
        private readonly ApplicationDbContext _context;

        public HomeService(IHomeRepository homeInterface, ApplicationDbContext context)
        {
            _homeRepository = homeInterface;
            _context = context;
        }

        public async Task<IEnumerable<ProductInWishListDTO>> ListarProdutosDaListaDeDesejos([FromQuery] int idCliente)
        {
            if (idCliente == 0)
                throw new Exception("Id inválido");

            var listaDesejos = await _homeRepository.ObterListaDeDesejos(idCliente);

            if (listaDesejos == null)
                throw new Exception("Lista de desejos não encontrada");

            return listaDesejos;
        }

        public async Task<IEnumerable<tbProdutoModel>> ListarProdutosRecomendados([FromQuery] int idCliente)
        {
            if (idCliente == 0)
                throw new Exception("Id inválido");

            var produtosRecomendados = await _homeRepository.ObterProdutosRecomendados(idCliente);

            return produtosRecomendados;
        }

        public async Task<IEnumerable<ProductsWithPromotionDTO>> ListarPromocoesDestaque()
        {
            var promocoesDestaque = await _homeRepository.ObterPromocoesDestaque();
            return promocoesDestaque;
        }

        public async Task<IEnumerable<tbProdutoModel>> ListarTodosOsProdutos()
        {
            var produtos = await _homeRepository.ObterTodosProdutos();
            return produtos;
        }

        public async Task<IEnumerable<NotificationForClientDTO>> ListarNotificacoesDoCliente([FromQuery] int idCliente)
        {
            if (idCliente == 0)
                throw new Exception("Id inválido");

            var notificacoes = await _homeRepository.ObterNotificacoesDoCliente(idCliente);

            return notificacoes;
        }

        public async Task<IEnumerable<tbProdutoModel>> ListarProdutosMaisVendidos()
        {
            var maisVendidos = await _homeRepository.ObterProdutosMaisVendidos();
            return maisVendidos;
        }

        public async Task<ICollection<NotificationForClientDTO>> ListarNotificacoesNaoLidasDoCliente(int idCliente)
        {
            var clienteExiste = await _homeRepository.VerificarSeClienteExiste(idCliente);

            if (!clienteExiste)
                throw new Exception("Cliente não existente");

            var notificacoesNaoLidas = await _homeRepository.ObterNotificacoesNaoLidasDoCliente(idCliente);

            if (!notificacoesNaoLidas.Any())
                return null;

            return notificacoesNaoLidas;
        }

        public async Task<IEnumerable<tbProdutoModel>> ListarProdutoPorId(int idProduto)
        {
            var produtoExiste = await _homeRepository.VerificarSeProdutoExiste(idProduto);

            if (!produtoExiste)
                throw new Exception("Produto inexistente");

            var produto = await _homeRepository.ObterProdutoPorId(idProduto);

            if (produto == null || !produto.Any())
                return null;

            return produto;
        }

        public async Task<ProductInWishListDTO> AdicionarProdutoNaListaDeDesejos(int idProduto, int idCliente)
        {
            if (idProduto == 0 || idCliente == 0)
                throw new Exception("Preencha todos os campos");

            var produtoExiste = await _homeRepository.VerificarSeProdutoExiste(idProduto);
            if (!produtoExiste)
                throw new Exception("Produto inexistente");

            var clienteExiste = await _homeRepository.VerificarSeClienteExiste(idCliente);
            if (!clienteExiste)
                throw new Exception("Cliente inexistente");

            var produtoAdicionado = await _homeRepository.AdicionarProdutoNaListaDeDesejos(idProduto, idCliente);
            return produtoAdicionado;
        }

        public async Task<IEnumerable<GetClientCarDTO>> ListarProdutosDoCarrinho(int idCliente)
        {
            var produtosCarrinho = await _homeRepository.ObterProdutosDoCarrinho(idCliente);

            if (!produtosCarrinho.Any())
                throw new Exception("Carrinho vazio");

            return produtosCarrinho;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> ListarTiposDeContaDoClienteVendedor(int idCliente)
        {
            var tipoConta = await _homeRepository.ObterTipoContaClienteVendedor(idCliente);
            return tipoConta;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> ListarTiposDeContaDoClienteEntregador(int idCliente)
        {
            var tipoConta = await _homeRepository.ObterTipoContaClienteEntregador(idCliente);
            return tipoConta;
        }

        public async Task<IEnumerable<SearchProductsByCategoryDTO>> ListarProdutosPorCategoria(int idCategoria)
        {
            var produtosCategoria = await _homeRepository.BuscarProdutosPorCategoria(idCategoria);
            return produtosCategoria;
        }

        public async Task<IEnumerable<GetClientByIdDTO>> ListarClientePorId(int idCliente)
        {
            var clienteExiste = await _homeRepository.VerificarSeClienteExiste(idCliente);

            if (!clienteExiste)
                throw new Exception("Cliente inexistente");

            var cliente = await _homeRepository.ObterClientePorId(idCliente);

            return cliente;
        }

        public async Task<IEnumerable<tbPromocaoModel>> ListarPromocoesDoProduto(int idProduto)
        {
            var promocaoExiste = await _homeRepository.VerificarSePromocaoExiste(idProduto);

            if (!promocaoExiste)
                return null;

            var promocoes = await _homeRepository.ObterPromocoesPorProduto(idProduto);

            return promocoes;
        }

        public async Task<tbCarrinhoModel> RemoverProdutoDoCarrinho(int idCliente, int idProduto)
        {
            var resultado = await _homeRepository.RemoverProdutoDoCarrinho(idCliente, idProduto);
            return resultado;
        }

        public async Task<tbLista_DesejoModel> RemoverProdutoDaListaDeDesejos(int idCliente, int idProduto)
        {
            var resultado = await _homeRepository.RemoverProdutoDaListaDeDesejos(idCliente, idProduto);
            return resultado;
        }

        public async Task<tbNotificacao_ClienteModel> MarcarNotificacaoComoLida(int idCliente, int idNotificacao)
        {
            var resultado = await _homeRepository.MarcarNotificacaoComoLida(idCliente, idNotificacao);
            return resultado;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> TipoDeConta(int idCliente)
        {
            var tipoContaVendedor = await _homeRepository.ObterTipoContaClienteVendedor(idCliente);
            if (tipoContaVendedor.Any()) return tipoContaVendedor;

            var tipoContaEntregador = await _homeRepository.ObterTipoContaClienteEntregador(idCliente);
            return tipoContaEntregador;

        }
    }
}
