using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
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

        public async Task<IEnumerable<tbProdutoModel>> ForYou([FromQuery] int id)
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

        [HttpGet("search")]
        public async Task<IEnumerable<tbProdutoModel>> Search([FromQuery] string name)
        {
            var search = await _context.produtos.Where(p => EF.Functions.Like(p.Nome, $"%{name}%")).ToListAsync();
            return search;
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
        public async Task<IEnumerable<ProductInWishListDTO>> WishList([FromQuery] int id)
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
        public async Task<IEnumerable<NotificationForClientDTO>> Notifications([FromQuery] int id)
        {
            var clientUsing = await _context.clientes.Where(c => c.IdCliente == id).FirstOrDefaultAsync();
            var notifications = await _context.notificacoesClientes.Where(nc => nc.IdCliente == id)
                .Select(nc => new NotificationForClientDTO
                {
                    DataCriacao = nc.DataCriacao,
                    Mensagem = nc.Notificacao.Mensagem,
                    IsLida = nc.IsLida
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

        public async Task<IEnumerable<NotificationForClientDTO>> NotificationsUnread([FromQuery] int id)
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
                    IdVendedor = p.IdVendedor ?? 0

                }).ToListAsync();

            return getProductById;
        }
    }
}

