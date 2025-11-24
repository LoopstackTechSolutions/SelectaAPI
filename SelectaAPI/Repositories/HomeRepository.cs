using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;
using System.Linq;

namespace SelectaAPI.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<tbProdutoModel>> ObterProdutosRecomendados(int idCliente)
        {
            var cliente = await _context.clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            var pedidosDoCliente = await _context.pedidos
                .Where(p => p.IdComprador == idCliente)
                .Select(p => p.IdPedido)
                .ToListAsync();

            var produtosComprados = await _context.produtosPedidos
                .Where(pp => pedidosDoCliente.Contains(pp.IdPedido))
                .Select(pp => pp.IdProduto)
                .Distinct()
                .ToListAsync();

            var categoriasProdutosComprados = await _context.categoriasProdutos
                .Where(cp => produtosComprados.Contains(cp.IdProduto))
                .Select(cp => cp.IdCategoria)
                .Distinct()
                .ToListAsync();

            var produtosRecomendadosIds = await _context.categoriasProdutos
                .Where(cp =>
                    categoriasProdutosComprados.Contains(cp.IdCategoria) &&
                    !produtosComprados.Contains(cp.IdProduto))
                .Select(cp => cp.IdProduto)
                .Distinct()
                .Take(20)
                .ToListAsync();

            var produtosRecomendados = await _context.produtos
                .Where(p => produtosRecomendadosIds.Contains(p.IdProduto))
                .Select(p => new tbProdutoModel
                {
                    IdProduto = p.IdProduto,
                    Nome = p.Nome,
                    PrecoUnitario = p.PrecoUnitario,
                    Peso = p.Peso,
                    Condicao = p.Condicao,
                    Quantidade = p.Quantidade
                })
                .Take(20)
                .ToListAsync();

            return produtosRecomendados;
        }

        public async Task<IEnumerable<tbPromocaoModel>> ObterPromocoesPorProduto(int idProduto)
        {
            var promocoes = await _context.promocoes
                .Where(prom => prom.IdProduto == idProduto)
                .Select(prom => new tbPromocaoModel
                {
                    IdProduto = prom.IdProduto,
                    IdPromocao = prom.IdPromocao,
                    Desconto = prom.Desconto,
                    ValidaAte = prom.ValidaAte,
                    Status = prom.Status,
                })
                .ToListAsync();

            return promocoes;
        }

        public async Task<IEnumerable<ProductsWithPromotionDTO>> ObterPromocoesDestaque()
        {
            var produtosComPromocao = await _context.promocoes
                .Include(pr => pr.Produto)
                .Select(pr => new ProductsWithPromotionDTO
                {
                    IdProduto = pr.Produto.IdProduto,
                    Nome = pr.Produto.Nome,
                    PrecoUnitario = pr.Produto.PrecoUnitario,
                    Condicao = pr.Produto.Condicao,
                    Peso = pr.Produto.Peso ?? 0,
                    Quantidade = pr.Produto.Quantidade ?? 0,
                    Status = pr.Produto.Status,
                    ValidoAte = pr.ValidaAte,
                    Desconto = pr.Desconto
                })
                .OrderBy(dto => dto.Desconto)
                .Take(20)
                .ToListAsync();

            return produtosComPromocao;
        }

        public async Task<IEnumerable<ProductInWishListDTO>> ObterListaDeDesejos(int idCliente)
        {
            var cliente = await _context.clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            var listaDesejos = await _context.listasDesejo
                .Where(ld => ld.IdCliente == idCliente)
                .Select(ld => new ProductInWishListDTO
                {
                    IdProduto = ld.Produto.IdProduto,
                    Nome = ld.Produto.Nome,
                    PrecoUnitario = ld.Produto.PrecoUnitario,
                    Quantidade = ld.Produto.Quantidade ?? 0,
                    Condicao = ld.Produto.Condicao,
                    Status = ld.Produto.Status,
                    Peso = ld.Produto.Peso
                })
                .Take(20)
                .ToListAsync();

            return listaDesejos;
        }

        public async Task<IEnumerable<NotificationForClientDTO>> ObterNotificacoesDoCliente(int idCliente)
        {
            var cliente = await _context.clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            var notificacoes = await _context.notificacoesClientes
                .Where(nc => nc.IdCliente == idCliente)
                .Select(nc => new NotificationForClientDTO
                {
                    DataCriacao = nc.DataCriacao,
                    Mensagem = nc.Notificacao.Mensagem,
                    IsLida = nc.IsLida,
                    IdContexto = nc.IdContexto,
                    Titulo = nc.Notificacao.TabelaContexto,
                    IdNotificacaoCliente = nc.IdNotificacaoCliente,
                })
                .ToListAsync();

            var notificacoesNaoLidas = await _context.notificacoesClientes
                .Where(nc => nc.IdCliente == idCliente && nc.IsLida != true)
                .ToListAsync();

            foreach (var notificacao in notificacoesNaoLidas)
            {
                notificacao.IsLida = true;
            }

            await _context.SaveChangesAsync();
            return notificacoes;
        }

        public async Task<ICollection<NotificationForClientDTO>> ObterNotificacoesNaoLidasDoCliente(int idCliente)
        {
            var notificacoesNaoLidas = await _context.notificacoesClientes
                .Where(nc => nc.IdCliente == idCliente && nc.IsLida == false)
                .Select(nc => new NotificationForClientDTO
                {
                    DataCriacao = nc.DataCriacao,
                    Mensagem = nc.Notificacao.Mensagem,
                    IsLida = nc.IsLida
                })
                .ToListAsync();

            return notificacoesNaoLidas;
        }

        public async Task<IEnumerable<tbProdutoModel>> ObterProdutosMaisVendidos()
        {
            var produtosVendidos = await _context.produtosPedidos
                .GroupBy(pp => pp.IdProduto)
                .Select(grupo => new
                {
                    IdProduto = grupo.Key,
                    QuantidadeVendida = grupo.Sum(pp => pp.Quantidade)
                })
                .OrderByDescending(r => r.QuantidadeVendida)
                .Join(
                    _context.produtos,
                    r => r.IdProduto,
                    produto => produto.IdProduto,
                    (r, produto) => new tbProdutoModel
                    {
                        IdProduto = produto.IdProduto,
                        Nome = produto.Nome,
                        Peso = produto.Peso,
                        Condicao = produto.Condicao,
                        PrecoUnitario = produto.PrecoUnitario,
                        Quantidade = r.QuantidadeVendida
                    })
                .Take(20)
                .ToListAsync();

            return produtosVendidos;
        }

        public async Task<IEnumerable<tbProdutoModel>> ObterTodosProdutos()
        {
            var produtos = await _context.produtos
                .Select(p => new tbProdutoModel
                {
                    IdProduto = p.IdProduto,
                    Nome = p.Nome,
                    Quantidade = p.Quantidade ?? 0,
                    PrecoUnitario = p.PrecoUnitario,
                    Condicao = p.Condicao,
                    Peso = p.Peso ?? 0,
                    Status = p.Status,
                    IdVendedor = p.IdVendedor ?? 0
                })
                .Take(20)
                .ToListAsync();

            return produtos;
        }

        public async Task<tbProdutoModel> ObterProdutoPorId(int idProduto)
        {
            var produto = await _context.produtos
                .FindAsync(idProduto);
            return produto;
        }

        public async Task<ProductInWishListDTO> AdicionarProdutoNaListaDeDesejos(int idProduto, int idCliente)
        {
            var novoItem = new tbLista_DesejoModel
            {
                IdProduto = idProduto,
                IdCliente = idCliente
            };

            _context.listasDesejo.Add(novoItem);
            await _context.SaveChangesAsync();

            var itemInserido = await _context.produtos
                .Where(p => p.IdProduto == idProduto)
                .Select(p => new ProductInWishListDTO
                {
                    IdProduto = idProduto,
                    Condicao = p.Condicao,
                    Nome = p.Nome,
                    PrecoUnitario = p.PrecoUnitario,
                    Peso = p.Peso ?? 0,
                    Status = p.Status,
                    Quantidade = p.Quantidade
                })
                .FirstOrDefaultAsync();

            return itemInserido;
        }

        public async Task<IEnumerable<GetClientCarDTO>> ObterProdutosDoCarrinho(int idCliente)
        {
            var produtos = await _context.carrinho
                .Where(c => c.IdCliente == idCliente)
                .Select(c => new GetClientCarDTO
                {
                    IdProduto = c.IdProduto,
                    PrecoUnitario = c.Produto.PrecoUnitario,
                    Peso = c.Produto.Peso,
                    Condicao = c.Produto.Condicao,
                    Nome = c.Produto.Nome,
                    Quantidade = c.Quantidade,
                })
                .ToListAsync();

            return produtos;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> ObterTipoContaClienteVendedor(int idCliente)
        {
            var contas = await _context.vendedores
                .Where(v => v.IdVendedor == idCliente)
                .Select(v => new TypeAccountOfClientDTO
                {
                    IdCliente = idCliente,
                    Nome = v.Cliente.Nome
                })
                .ToListAsync();

            foreach (var conta in contas)
            {
                conta.isVendedor = true;
                conta.isEntregador = false;
            }

            return contas;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> ObterTipoContaClienteEntregador(int idCliente)
        {
            var contas = await _context.entregadores
                .Where(e => e.IdEntregador == idCliente)
                .Select(e => new TypeAccountOfClientDTO
                {
                    IdCliente = idCliente,
                    Nome = e.Cliente.Nome
                })
                .ToListAsync();

            foreach (var conta in contas)
            {
                conta.isVendedor = false;
                conta.isEntregador = true;
            }

            return contas;
        }

        public async Task<IEnumerable<SearchProductsByCategoryDTO>> BuscarProdutosPorCategoria(int idCategoria)
        {
            var produtos = await _context.categoriaProdutos
                .Where(cp => cp.IdCategoria == idCategoria)
                .Select(cp => new SearchProductsByCategoryDTO
                {
                    Nome = cp.Produto.Nome,
                    PrecoUnitario = cp.Produto.PrecoUnitario,
                    Condicao = cp.Produto.Condicao,
                    Status = cp.Produto.Status,
                    Peso = cp.Produto.Peso,
                    Quantidade = cp.Produto.Quantidade
                })
                .ToListAsync();

            return produtos;
        }

        public async Task<IEnumerable<GetClientByIdDTO>> ObterClientePorId(int idCliente)
        {
            var cliente = await _context.clientes
                .Where(c => c.IdCliente == idCliente)
                .Select(c => new GetClientByIdDTO
                {
                    IdCliente = c.IdCliente,
                    Email = c.Email,
                    Nome = c.Nome,
                    Saldo = c.Saldo,
                })
                .ToArrayAsync();

            return cliente;
        }

        public async Task<tbCarrinhoModel> RemoverProdutoDoCarrinho(int idCliente, int idProduto)
        {
            var item = await _context.carrinho
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente && c.IdProduto == idProduto);

            if (item == null)
                throw new Exception("Produto não encontrado no carrinho deste cliente.");

            _context.carrinho.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<tbLista_DesejoModel> RemoverProdutoDaListaDeDesejos(int idCliente, int idProduto)
        {
            var item = await _context.listasDesejo
                .FirstOrDefaultAsync(l => l.IdCliente == idCliente && l.IdProduto == idProduto);

            if (item == null)
                throw new Exception("Produto não encontrado na lista de desejos deste cliente.");

            _context.listasDesejo.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<tbNotificacao_ClienteModel> MarcarNotificacaoComoLida(int idCliente, int idNotificacao)
        {
            var notificacao = await _context.notificacoesClientes
                .FirstOrDefaultAsync(nc => nc.IdCliente == idCliente && nc.IdNotificacao == idNotificacao);

            if (notificacao != null)
            {
                notificacao.IsLida = true;
                await _context.SaveChangesAsync();
                return notificacao;
            }

            return new tbNotificacao_ClienteModel
            {
                IsLida = true,
                IdCliente = idCliente,
                IdNotificacao = idNotificacao
            };
        }

        public async Task<bool> VerificarSeClienteExiste(int idCliente)
        {
            return await _context.clientes.AnyAsync(c => c.IdCliente == idCliente);
        }

        public async Task<bool> VerificarSeProdutoExiste(int idProduto)
        {
            return await _context.produtos.AnyAsync(p => p.IdProduto == idProduto);
        }

        public async Task<bool> VerificarSePromocaoExiste(int idProduto)
        {
            return await _context.promocoes.AnyAsync(pr => pr.IdProduto == idProduto);
        }
    }
}
