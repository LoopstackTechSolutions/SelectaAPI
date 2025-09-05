using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<tbCategoriaModel>> GetAllCategories();
    }
}
