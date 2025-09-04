using SelectaAPI.Models;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp;
using SelectaAPI.DTOs;

namespace SelectaAPI.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeInterface)
        {
            _homeRepository = homeInterface;
        }
        public async Task<IEnumerable<tbProdutoModel>> Search([FromQuery]string name)
        {
                var search = await _homeRepository.Search(name);
                return search;
        }

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
    }
}
