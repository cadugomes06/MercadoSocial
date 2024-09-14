using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercadoSocial.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepositorio _UserRepositorio;
        public UserController(IUserRepositorio userRepositorio)
        {
            _UserRepositorio = userRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            List<UserModel> users = await _UserRepositorio.GetAllUsers();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            try
            {
                if (ModelState.IsValid && user != null)
                {
                    await _UserRepositorio.CreateUser(user);
                    TempData["SuccessMensage"] = "Usuário criado com sucesso";
                    return RedirectToAction("Index");
                }
                return View(user);
            }
            catch (Exception erro)
            {
                TempData["ErroMessage"] = "Ops... Houve um erro ao criar o seu novo produto. " + erro.Message;
                return RedirectToAction("Index");
            }
        }






    }
}
