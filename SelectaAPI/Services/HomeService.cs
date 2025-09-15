using SelectaAPI.Models;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp;
using SelectaAPI.DTOs;
using SelectaAPI.Database;
using Microsoft.EntityFrameworkCore;
using Amazon.Runtime.Internal.Util;

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
        /*
        public async Task<IEnumerable<tbProdutoModel>> Search([FromQuery]string name)
        {
                var search = await _homeRepository.Search(name);
                return search;
        }
        */

        public async Task<IEnumerable<ProductInWishListDTO>> WishList([FromQuery]int id)
        {
            if (id == null) throw new Exception("Id inválido");
            var wishList = await _homeRepository.WishList(id);
            if(wishList == null) throw new Exception("Lista de Desejos não encontrada");
            return wishList;
        }

        public async Task<IEnumerable<tbProdutoModel>> ForYou([FromQuery] int id)
        {
            if (id == 0) throw new Exception("Id inválido");
            var forYou = await _homeRepository.ForYou(id);

            return forYou;
        }
        public async Task<IEnumerable<ProductsWithPromotionDTO>> Highlights()
        {
            var HighLights = await _homeRepository.Highlights();
            return HighLights;
        }

        public async Task<IEnumerable<tbProdutoModel>> GetAll()
        {
            var getAll = await _homeRepository.GetAll();
            return getAll;
        }

        public async Task<IEnumerable<NotificationForClientDTO>> Notifications([FromQuery] int id)
        {
            if (id == null) throw new Exception("Id inválido");
            
            var notifications = await _homeRepository.Notifications(id);
            return notifications;
        }

        public async Task<IEnumerable<tbProdutoModel>> BestSellers()
        {
            var bestSeller = await _homeRepository.BestSellers();
            return bestSeller;
        }

        public async Task<ICollection<NotificationForClientDTO>> NotificationsUnread(int id)
        {

            var clientExists = await _context.clientes.AnyAsync(c => c.IdCliente == id);

            if (!clientExists) throw new Exception("Cliente não existente");

            var notificationUnread = await _homeRepository.NotificationsUnread(id);

            if (!notificationUnread.Any()) return notificationUnread = null;

            return notificationUnread;

        }

        public async Task<IEnumerable<tbProdutoModel>> GetProductByID(int id)
        {
            var productExists = await _context.produtos.AnyAsync(p => p.IdProduto == id);

            if (!productExists) throw new Exception("id do produto inexistente");

            var getProductById = await _homeRepository.GetProductByID(id);

            if (getProductById == null) throw new Exception("id do produto nulo");

            if (!getProductById.Any()) return getProductById = null;

            return getProductById;
        }

        public async Task<ProductInWishListDTO> AddProductInWishList(int id, int idCliente)
        {
            if (id == null || idCliente == null) throw new Exception("preencha todos os campos");
            var productExist = await _context.produtos.AnyAsync(p => p.IdProduto == id);
            if (!productExist) throw new Exception("produto inexistente");

            var clientExist = await _context.clientes.AnyAsync(c => c.IdCliente == idCliente);
            if (!clientExist) throw new Exception("cliente inexistente");

            var addProductInWishList = await _homeRepository.AddProductInWishList(id, idCliente);
            return addProductInWishList;
        }

        public async Task<IEnumerable<GetClientCarDTO>> GetProductsInCartOfClient(int idClient)
        {
            var getProductsInCar = await _homeRepository.GetProductsInCartOfClient(idClient);
            if (!getProductsInCar.Any()) throw new Exception("Lista vazia");
            return getProductsInCar;
        }

        public async Task<IEnumerable<TypeAccountOfClientDTO>> GetTypeAccountOfClient(int idClient)
        {
           var getTypeAccountSalesPerson = await _homeRepository.GetTypeAccountOfClientSalesPerson(idClient);
            if (getTypeAccountSalesPerson.Any()) return getTypeAccountSalesPerson;

            var getTypeAccountDeliveryPerson = await _homeRepository.GetTypeAccountOfClientDeliveryPerson(idClient); 
                    return getTypeAccountDeliveryPerson;
        }

        public async Task<IEnumerable<SearchProductsByCategoryDTO>> SearchProductByCategory(int id)
        {
            var getProductsByCategory = await _homeRepository.SearchProductByCategory(id);
            return getProductsByCategory;
        }

        public async Task<IEnumerable<GetClientByIdDTO>> GetClientById(int id)
        {
            var clientExists = await _context.clientes
                .AnyAsync(c => c.IdCliente == id);
            if (!clientExists) throw new Exception("cliente inexistente");

            var getClientById = await _homeRepository.GetClientById(id);

            return getClientById;
        }
    }
}
