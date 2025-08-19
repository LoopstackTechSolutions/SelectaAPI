using Microsoft.AspNetCore.Mvc;

namespace SelectaAPI.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
