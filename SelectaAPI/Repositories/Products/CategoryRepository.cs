using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;

namespace SelectaAPI.Repositories.Products
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
            var getAll = await _context.categorias.Select(c => new
            {
               c.IdCategoria,
                c.Nome
            }).ToListAsync();
            return (IEnumerable<tbCategoriaModel>)getAll;
        }
    }
}
