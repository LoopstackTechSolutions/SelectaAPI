using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.ProductsInterface
{
    public interface ICategoryService
    {
        Task<IEnumerable<tbCategoriaModel>> TodasAsCategorias();
    }
}
