using MercadoSocial.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MercadoSocial.Controllers
{
    public class RestrictedController : Controller
    {
        [PageToUserLogged]
        public IActionResult Index()
        {
            return View();
        }
    }
}
