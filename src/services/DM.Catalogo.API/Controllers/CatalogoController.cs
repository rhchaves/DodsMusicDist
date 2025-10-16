using Microsoft.AspNetCore.Mvc;

namespace DM.Catalogo.API.Controllers
{
    public class CatalogoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
