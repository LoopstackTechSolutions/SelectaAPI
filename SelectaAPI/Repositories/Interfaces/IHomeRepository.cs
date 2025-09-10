using SelectaAPI.Models;
using SelectaAPI.DTOs;
using System.Text.Json.Serialization.Metadata;

namespace SelectaAPI.Repository.Interfaces
{
    public interface IHomeRepository
    {
        Task<IEnumerable<tbProdutoModel>> Search(string name);
        Task<IEnumerable<ProductInWishListDTO>> WishList(int id);
        Task<IEnumerable<tbProdutoModel>> ForYou(int id);
        Task<IEnumerable<ProductsWithPromotionDTO>> Highlights();
        Task<IEnumerable<NotificationForClientDTO>> Notifications(int id);
        Task<ICollection<NotificationForClientDTO>> NotificationsUnread(int id);
        Task<IEnumerable<tbProdutoModel>> BestSellers();
        Task<IEnumerable<tbProdutoModel>> GetProductByID(int id);
        Task<ProductInWishListDTO> AddProductInWishList(int id, int idCliente);
        Task<IEnumerable<GetClientCarDTO>> GetProductsInCarOfClient(int idClient);
        Task<IEnumerable<TypeAccountOfClientDTO>> GetTypeAccountOfClientSalesPerson(int idClient);
        Task<IEnumerable<TypeAccountOfClientDTO>> GetTypeAccountOfClientDeliveryPerson(int idClient);
        Task<IEnumerable<SearchProductsByCategoryDTO>> SearchProductByCategory(int id);
        Task<IEnumerable<tbProdutoModel>> GetAll();
    }
}
