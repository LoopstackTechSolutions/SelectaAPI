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
        Task<IEnumerable<NotificationForClientDTO>> NotificationsUnread(int id);
        Task<IEnumerable<tbProdutoModel>> BestSellers();
        Task<IEnumerable<tbProdutoModel>> GetProductByID(int id);
        Task<ProductInWishListDTO> AddProductInWishList(int id, int idCliente);
        Task<IEnumerable<tbProdutoModel>> GetAll();
    }
}
