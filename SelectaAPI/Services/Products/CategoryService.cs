using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Services.Interfaces.ProductsInterface;

namespace SelectaAPI.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<tbCategoriaModel>> TodasAsCategorias()
        {
            var getAll = await _categoryRepository.TodasAsCategorias();
            return getAll;
        }
    }
}
