using Microsoft.AspNetCore.Mvc;

namespace SelectaAPI.Controllers
{
    public class FuncionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
