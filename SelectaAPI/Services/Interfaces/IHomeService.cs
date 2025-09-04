using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<tbProdutoModel>> Search(string name);
        Task<IEnumerable<ProductInWishListDTO>> WishList(int id);
        Task<IEnumerable<tbProdutoModel>> ForYou(int id);
        Task<IEnumerable<ProductsWithPromotionDTO>> Highlights();
        Task<IEnumerable<NotificationForClientDTO>> Notifications(int id);
        Task<IEnumerable<tbProdutoModel>> BestSellers();
    }
}
