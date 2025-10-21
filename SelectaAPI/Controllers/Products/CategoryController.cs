using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.Services.Interfaces.ProductsInterface;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
       
        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var getAll = await _categoryService.GetAllCategories();
                return Ok(getAll);
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"erro no servidor {ex.Message}");
            }
        }
    }
}
