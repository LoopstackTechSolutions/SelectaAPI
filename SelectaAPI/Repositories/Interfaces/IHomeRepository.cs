using SelectaAPI.Models;
using SelectaAPI.DTOs;

namespace SelectaAPI.Repository.Interfaces
{
    public interface IHomeRepository
    {
        Task<IEnumerable<tbProdutoModel>> Search(string name);
        Task<IEnumerable<ProductInWishListDTO>> WishList(int id);
        Task<IEnumerable<tbProdutoModel>> ForYou(int id);
        Task<IEnumerable<ProductsWithPromotionDTO>> Highlights();
        Task<IEnumerable<NotificationForClientDTO>> Notifications(int id);
        Task<IEnumerable<tbProdutoModel>> BestSellers();
        Task<IEnumerable<tbProdutoModel>> GetAll();
    }
}
