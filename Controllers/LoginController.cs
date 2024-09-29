using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MercadoSocial.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserRepositorio _userRepositorio;
        public LoginController(IUserRepositorio userRepositorio)
        {
            _userRepositorio = userRepositorio;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logar(LoginModel loginModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    UserModel user = await _userRepositorio.SearchUserLogin(loginModel.Login);

                    if (user != null)
                    {

                        if (user.PasswordIsValid(loginModel.Password))
                        {

                          return RedirectToAction("Index", "Home");
                        }

                        TempData["ErroMessage"] = "Usuário e/ou senha inválido(s).;
                    }
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErroMessage"] = "Houve um erro ao tentar realizar o login." + ex.Message;
                return RedirectToAction("Index");
            }

        }



    }
}
