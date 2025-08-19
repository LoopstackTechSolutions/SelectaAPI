using Microsoft.AspNetCore.Mvc;

namespace SelectaAPI.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
