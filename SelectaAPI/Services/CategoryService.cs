using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;

namespace SelectaAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<tbCategoriaModel>> GetAllCategories()
        {
            var getAll = await _categoryRepository.GetAllCategories();
            return getAll;
        }
    }
}
