using MercadoSocial.Filters;
using MercadoSocial.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MercadoSocial.Controllers
{
    public class HomeController : Controller
    {
        [PageToUserLogged]
        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
