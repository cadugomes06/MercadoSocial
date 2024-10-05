using MercadoSocial.Helper;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MercadoSocial.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserRepositorio _userRepositorio;
        private readonly ISessao _sessao;
        public LoginController(IUserRepositorio userRepositorio, ISessao sessao)
        {
            _userRepositorio = userRepositorio;
            _sessao = sessao;
        }


        public IActionResult Index()
        {
            //Se o usuário estiver logado, retornar para home
            if(_sessao.SearchSectionUser() != null)
            {
                return RedirectToAction("Index", "Home");
            }
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
                            _sessao.createSectionUser(user);
                          return RedirectToAction("Index", "Home");
                        }

                        TempData["ErroMessage"] = "Usuário e/ou senha inválido(s).";
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


        public ActionResult Logout()
        {
            _sessao.RemoveSectionUser();
            return RedirectToAction("Index", "Login");
        }



    }
}
