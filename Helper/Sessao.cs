using MercadoSocial.Models;
using System.Text.Json;

namespace MercadoSocial.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;
        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public UserModel SearchSectionUser()
        {
            string sectionUser = _httpContext.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if(string.IsNullOrEmpty(sectionUser)) return null;
            
           return JsonSerializer.Deserialize<UserModel>(sectionUser);
        }

        public void createSectionUser(UserModel user)
        {
            string value = JsonSerializer.Serialize(user);
            _httpContext.HttpContext.Session.SetString("sessaoUsuarioLogado", value);
        }

        public void RemoveSectionUser()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuarioLogado");
        }

    }
}
