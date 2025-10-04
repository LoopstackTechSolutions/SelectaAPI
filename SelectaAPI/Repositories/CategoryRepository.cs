using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;

namespace SelectaAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<tbCategoriaModel>> GetAllCategories()
        {
            return await _context.categorias.ToListAsync();
        }

    }
}
