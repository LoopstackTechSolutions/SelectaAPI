using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.ProductsInterface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<tbCategoriaModel>> GetAllCategories();
    }
}
