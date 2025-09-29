using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SelectaAPI.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<tbProdutoModel>> ForYou(int id)
        {
            var clientUsing = await _context.clientes.Where(c => c.IdCliente == id).FirstOrDefaultAsync();
            var purchased = await _context.pedidos
             .Where(p => p.IdComprador == id)
                .Select(p => p.IdPedido)
                .ToListAsync();

            var purchasedProduct = await _context.produtosPedidos
                .Where(pp => purchased.Contains(pp.IdPedido))
                .Select(pp => pp.IdProduto)
                .Distinct()
                .ToListAsync();


            var purchasedCategory = await _context.categoriasProdutos
                .Where(cp => purchasedProduct.Contains(cp.IdProduto))
                .Select(cp => cp.IdCategoria)
                .Distinct()
                .ToListAsync();

            var recommendedProducts = await _context.categoriasProdutos
                .Where(cp => purchasedCategory.Contains(cp.IdCategoria)
                             && !purchasedProduct.Contains(cp.IdProduto))
                .Select(cp => cp.IdProduto)
                .Distinct()
                .Take(20)
                .ToListAsync();

            var recommendationList = await _context.produtos
                .Where(p => recommendedProducts.Contains(p.IdProduto))
                .Select(p => new tbProdutoModel
                {
                    IdProduto = p.IdProduto,
                    Nome = p.Nome,
                    PrecoUnitario = p.PrecoUnitario,
                    Peso = p.Peso,
                    Condicao = p.Condicao,
                    Quantidade = p.Quantidade
                })
                .ToListAsync();
            return recommendationList;
        }

        /*
        public async Task<IEnumerable<tbProdutoModel>> Search(string query)
        {

            var word = query.Split("", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim().ToLower()).ToList();

            var filterProducts = await _context.categoriaProdutos.Select(p => new
            {
                Produto = p,
                MatchCount = word.Count(w =>
                    EF.Functions.Like(p.Produto.Nome.ToLower(), $"%{w}%") ||
                    EF.Functions.Like(p.Categoria.Nome.ToLower(), $"%{w}%")
                    )
            })
                .Where(x => x.MatchCount > 0)
                .OrderByDescending(x => x.MatchCount)
                .ThenByDescending(x => x.Produto)
                .ToListAsync();
            if (filterProducts.Count < 20)
            {
                var missing = 20 - filterProducts.Count;

                var produtcsRandom = await _context.produtos
                    .Where(p => !filterProducts.Select(r => r.Produto.IdProduto).Contains(p.IdProduto))
                    .OrderBy(r => Guid.NewGuid())
                    .Take(missing)
                    .ToListAsync();

                produtcsRandom.AddRange(produtcsRandom);
            }

            return filterProducts;

        }
        */

        public async Task<IEnumerable<tbPromocaoModel>> GetAllPromotionOfProduct(int id)
        {
            var getPromotion = await _context.promocoes.Where(p => p.IdProduto == id)
                .Select(p => new tbPromocaoModel
                {
                    IdProduto = p.IdProduto,
                    IdPromocao = p.IdPromocao,
                    Desconto = p.Desconto,
                    ValidaAte = p.ValidaAte,
                    Status = p.Status,
                })
                .ToListAsync();
            return getPromotion;
        }

        public async Task<IEnumerable<ProductsWithPromotionDTO>> Highlights()
        {
            var productsPromotion = await _context.promocoes
           .Include(pp => pp.Produto)
           .Select(pp => new ProductsWithPromotionDTO
           {
               IdProduto = pp.Produto.IdProduto,
               Nome = pp.Produto.Nome,
               PrecoUnitario = pp.Produto.PrecoUnitario,
               Condicao = pp.Produto.Condicao,
               Peso = pp.Produto.Peso ?? 0,
               Quantidade = pp.Produto.Quantidade ?? 0,
               Status = pp.Produto.Status,
 
               ValidoAte = pp.ValidaAte,
               Desconto = pp.Desconto
           })
           .OrderBy(pp => pp.Desconto)
           .ToListAsync();
            return productsPromotion;
        }
        public async Task<IEnumerable<ProductInWishListDTO>> WishList(int id)
        {
            var clientUsing = await _context.clientes.Where(c => c.IdCliente == id).FirstOrDefaultAsync();
            var wishlistProducts = await _context.listasDesejo
      .Where(l => l.IdCliente == id)
      .Select(l => new ProductInWishListDTO
      {
          IdProduto = l.Produto.IdProduto,
          Nome = l.Produto.Nome,
          PrecoUnitario = l.Produto.PrecoUnitario,
          Quantidade = l.Produto.Quantidade ?? 0,
          Condicao = l.Produto.Condicao,
          Status = l.Produto.Status,
          Peso = l.Produto.Peso
      })
      .ToListAsync();

            return wishlistProducts;
        }
        public async Task<IEnumerable<NotificationForClientDTO>> Notifications(int id)
        {
            var clientUsing = await _context.clientes.Where(c => c.IdCliente == id).FirstOrDefaultAsync();
            var notifications = await _context.notificacoesClientes.Where(nc => nc.IdCliente == id)
                .Select(nc => new NotificationForClientDTO
                {
                    DataCriacao = nc.DataCriacao,
                    Mensagem = nc.Notificacao.Mensagem,
                    IsLida = nc.IsLida,
                    IdContexto = nc.IdContexto,
                    Titulo = nc.Notificacao.TabelaContexto,
                    IdNotificacaoCliente = nc.IdNotificacaoCliente,
                }).ToListAsync();

            var notificationReadUpdate = await _context.notificacoesClientes.
                Where(nc => nc.IdCliente == id && nc.IsLida != true).
                ToListAsync();

            foreach (var notification in notificationReadUpdate)
            {
                notification.IsLida = true;
            }

            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<ICollection<NotificationForClientDTO>> NotificationsUnread(int id)
        {
            var notificationsUnread = await _context.notificacoesClientes
            .Where(nc => nc.IdCliente == id && nc.IsLida == false)
                 .Select(nc => new NotificationForClientDTO
                 {
                     DataCriacao = nc.DataCriacao,
                     Mensagem = nc.Notificacao.Mensagem,
                     IsLida = nc.IsLida
                 }).ToListAsync();

            return notificationsUnread;
        }

        public async Task<IEnumerable<tbProdutoModel>> BestSellers()
        {
            var bestSellers = await _context.produtosPedidos.GroupBy(pp => pp.IdProduto)
               .Select(bs => new
               {
                   IdProduto = bs.Key,
                   quantitySold = bs.Sum(pp => pp.Quantidade)
               })
               .OrderByDescending(bs => bs.quantitySold)
               .Join(_context.produtos,
                    bs => bs.IdProduto,
                     p => p.IdProduto,
                    (bs, p) => new tbProdutoModel
                    {
                        IdProduto = p.IdProduto,
                        Nome = p.Nome,
                        Peso = p.Peso,
                        Condicao = p.Condicao,
                        PrecoUnitario = p.PrecoUnitario,
                        Quantidade = bs.quantitySold
                    }).
          Take(20)
         .ToListAsync();
            return bestSellers;
        }
        public async Task<IEnumerable<tbProdutoModel>> GetAll()
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
                .ToListAsync();
                return produtos;
        }

        public async Task<IEnumerable<tbProdutoModel>> GetProductByID(int id)
        {
            var getProductById = await _context.produtos.Where(p => p.IdProduto == id).
                Select(p => new tbProdutoModel
                {
                    IdProduto = p.IdProduto,
                    Nome = p.Nome,
                    Quantidade = p.Quantidade ?? 0,
                    PrecoUnitario = p.PrecoUnitario,
                    Condicao = p.Condicao,
                    Peso = p.Peso ?? 0,
                    Status = p.Status,
                    Descricao = p.Descricao,
                    IdVendedor = p.IdVendedor ?? 0

                }).ToListAsync();

            return getProductById;
        }

        public async Task<ProductInWishListDTO> AddProductInWishList(int id, int idCliente)
        {
            var addProductInWishList = new tbLista_DesejoModel
            {
                IdProduto = id,
                IdCliente = idCliente
            };

             _context.listasDesejo.Add(addProductInWishList);
            await _context.SaveChangesAsync();

            var response = await _context.produtos.Where(l => l.IdProduto == id)
                .Select(l => new ProductInWishListDTO
                {
                    IdProduto = id,
                    Condicao = l.Condicao,
                    Nome = l.Nome,
                    PrecoUnitario = l.PrecoUnitario,
                    Peso = l.Peso ?? 0,
                    Status = l.Status,
                    Quantidade = l.Quantidade
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<IEnumerable<GetClientCarDTO>> GetProductsInCartOfClient(int idClient)
        {
            var getProductsInCar = await _context.carrinho.Where(c => c.IdCliente == idClient)
                .Select(c => new GetClientCarDTO
                {
                    IdProduto = c.IdProduto,
                    PrecoUnitario = c.Produto.PrecoUnitario,
                    Peso = c.Produto.Peso,
                    Condicao = c.Produto.Condicao,
                    Nome = c.Produto.Nome,
                    Quantidade = c.Quantidade,
                }).ToListAsync();

            return getProductsInCar;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> GetTypeAccountOfClientSalesPerson(int idClient)
        {
            var getSalesPerson = await _context.vendedores.Where(v => v.IdVendedor == idClient)
                .Select(v => new TypeAccountOfClientDTO
                {
                    IdCliente = idClient,
                    Nome = v.Cliente.Nome
                }).ToListAsync();
            foreach (var salesPerson in getSalesPerson)
            {
                salesPerson.isVendedor = true;
                salesPerson.isEntregador = false;
            }
            return getSalesPerson;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> GetTypeAccountOfClientDeliveryPerson(int idClient)
        {
            var getDeliveryPerson = await _context.entregadores.Where(d => d.IdEntregador == idClient)
                .Select(v => new TypeAccountOfClientDTO
                {
                    IdCliente = idClient,
                    Nome = v.Cliente.Nome
                }).ToListAsync();
            foreach (var deliveryPerson in getDeliveryPerson)
            {
                deliveryPerson.isVendedor = false;
                deliveryPerson.isEntregador = true;
            }
            return getDeliveryPerson;
        }

        public async Task<IEnumerable<SearchProductsByCategoryDTO>> SearchProductByCategory(int id)
        {
            var getProductsInCategory = await _context.categoriaProdutos.Where(cp => cp.IdCategoria == id)
                .Select(cp => new SearchProductsByCategoryDTO
                {
                    Nome = cp.Produto.Nome,
                    PrecoUnitario = cp.Produto.PrecoUnitario,
                    Condicao = cp.Produto.Condicao,
                    Status = cp.Produto.Status,
                    Peso = cp.Produto.Peso,
                    Quantidade = cp.Produto.Quantidade
                }).ToListAsync();
            return getProductsInCategory;
        }

        public async Task<IEnumerable<GetClientByIdDTO>> GetClientById(int id)
        {
            var getClientById = await _context.clientes
                .Where(c => c.IdCliente == id)
                 .Select(c => new GetClientByIdDTO
                 {
                     IdCliente = c.IdCliente,
                     Email = c.Email,
                     Nome = c.Nome,
                     Saldo = c.Saldo,
                 }).ToArrayAsync();

            return getClientById;
        }
    }
}

