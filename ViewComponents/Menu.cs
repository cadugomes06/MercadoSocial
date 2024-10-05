using MercadoSocial.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MercadoSocial.ViewComponents
{
    public class Menu : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sectionUser = HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sectionUser)) return null;

            UserModel user = JsonSerializer.Deserialize<UserModel>(sectionUser);

            return View(user);
        }

    }
}
