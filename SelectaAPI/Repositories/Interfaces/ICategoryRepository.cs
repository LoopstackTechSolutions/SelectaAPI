using SelectaAPI.Models;

namespace SelectaAPI.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<tbCategoriaModel>> GetAllCategories();
    }
}
