using MercadoSocial.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MercadoSocial.Controllers
{
    public class LoginController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logar(LoginModel loginModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (loginModel.Login == "caduzinho123" && loginModel.Password == "123456")
                    {
                      return RedirectToAction("Index", "Home");
                    }
                    TempData["ErroMessage"] = "Usuário e/ou senha inválido(s).";
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErroMessage"] = "Houve um erro ao tentar realizar o login." + ex.Message;
                return RedirectToAction("Index");
            }

            return View();
        }



    }
}
