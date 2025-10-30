using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces
{
    public interface IHomeService
    {
       //  Task<IEnumerable<tbProdutoModel>> Search(string name);
        Task<IEnumerable<ProductInWishListDTO>> WishList(int id);
        Task<IEnumerable<tbProdutoModel>> ForYou(int id);
        Task<IEnumerable<ProductsWithPromotionDTO>> Highlights();
        Task<IEnumerable<NotificationForClientDTO>> Notifications(int id);
        Task<ICollection<NotificationForClientDTO>> NotificationsUnread(int id);
        Task<IEnumerable<tbProdutoModel>> BestSellers();
        Task<IEnumerable<tbProdutoModel>> GetProductByID(int id);
        Task<ProductInWishListDTO> AddProductInWishList(int id, int idCliente);
        Task<IEnumerable<GetClientCarDTO>> GetProductsInCartOfClient(int idClient);
        Task<IEnumerable<GetClientByIdDTO>> GetClientById(int id);
        Task<IEnumerable<TypeAccountOfClientDTO>> GetTypeAccountOfClient(int idClient);
        Task<IEnumerable<SearchProductsByCategoryDTO>> SearchProductByCategory(int id);
        Task<IEnumerable<tbProdutoModel>> GetAll();
        Task<IEnumerable<tbPromocaoModel>> GetAllPromotionOfProduct(int id);
        Task<tbCarrinhoModel> RemoveProductOfCart(int idCliente, int idProduto);
        Task<tbLista_DesejoModel> RemoveProductOfWishList(int idCliente, int idProduto);
        Task<tbNotificacao_ClienteModel> NotificationsRead(int idCliente, int idNotificacao);
    }
}
