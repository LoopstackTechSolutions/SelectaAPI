using Microsoft.AspNetCore.Mvc;

namespace SelectaAPI.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
