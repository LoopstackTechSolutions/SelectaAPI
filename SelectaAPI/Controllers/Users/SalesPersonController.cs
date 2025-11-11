using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Services.Interfaces.UsersInterface;

namespace SelectaAPI.Controllers.Users
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class SalesPersonController : ControllerBase
    {
        private readonly ISalesPersonService _salesPersonService;

        public SalesPersonController(ISalesPersonService salesPersonService)
        {
         _salesPersonService = salesPersonService;   
        }

        [HttpGet("my-products")]
        public async Task<IActionResult> ListProductByPerson(int idVendedor)
        {
            try
            {
                var salesPerson = await _salesPersonService.MyProducts(idVendedor);
                return Ok(salesPerson);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }

        }
    }
}
