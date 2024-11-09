using MercadoSocial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace MercadoSocial.Filters
{
    public class PageRestrictedToUserAdmin : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string sectionUser = context.HttpContext.Session.GetString("sessaoUsuarioLogado");
            if (string.IsNullOrEmpty(sectionUser))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { {"Controller", "Login"}, {"Action", "Index"} }); 
            } 
            else
            {
                UserModel user = JsonSerializer.Deserialize<UserModel>(sectionUser);

                if(user == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Login" }, { "Action", "Index" } });
                }

                if (user.Perfil != MercadoSocial.Enums.PerfilEnum.admin) 
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Restricted" }, { "Action", "Index" } });
                }

            }
            base.OnActionExecuted(context);
        }



    }
}
