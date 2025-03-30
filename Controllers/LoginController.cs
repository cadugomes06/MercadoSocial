using MercadoSocial.Helper;
using MercadoSocial.Logger;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercadoSocial.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserRepositorio _userRepositorio;
        private readonly ISessao _sessao;
        private readonly ILoggerService _logger;
        public LoginController(IUserRepositorio userRepositorio, ISessao sessao, ILoggerService logger)
        {
            _userRepositorio = userRepositorio;
            _sessao = sessao;
            _logger = logger;
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
                            await _logger.CreateLogger("Login", $"Usuario: {user.Name}, logou.", user.Id);
 

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
            _logger.CreateLogger("LogOut", $"Usuario: {_sessao.SearchSectionUser().Name}, deslogou.", _sessao.SearchSectionUser().Id);
            _sessao.RemoveSectionUser();
            return RedirectToAction("Index", "Login");
        }



    }
}
