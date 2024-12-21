using MercadoSocial.Filters;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercadoSocial.Controllers
{
    [PageRestrictedToUserAdmin]
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

        public async Task<IActionResult> Edit(int id)
        {
            UserModel user = await _UserRepositorio.GerUserById(id);
            if(user == null)
            {
                throw new Exception("Houve um erro ao tentar locarlizar o usuário.");
            }
            return View(user);
        }

        [HttpGet]
        public async Task<UserModel> GetUserById(int id)
        {
            try
            {
                UserModel user = await _UserRepositorio.GerUserById(id);
                if(user != null)
                {
                    return user;                
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar o usuário");
            }
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

        [HttpPost]
        public async Task<ActionResult> EditUser(UserSemSenhaModel user)
        {
            UserModel newUser = null;
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }

            try
            {
                newUser = new UserModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Login = user.Login,
                    Perfil = user.Perfil
                };

                UserModel userUpdated = await _UserRepositorio.UpdateUser(newUser);
                if (userUpdated == null)
                {
                    throw new Exception("Houve um erro ao buscar o usuário.");
                }
                TempData["SuccessMessage"] = "Usuário editado com sucesso.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErroMessage"] = "Ops... Houve um erro ao criar o seu novo produto. " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    




    }
}
