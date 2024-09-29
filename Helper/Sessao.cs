using MercadoSocial.Models;
using Microsoft.AspNetCore.SignalR;

namespace MercadoSocial.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;
        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public void createSectionUser(UserModel user)
        {
            _httpContext.HttpContext.Session.SetString("sessaoUsuarioLogado", "");
        }

        public void RemoveSectionUser()
        {
            throw new NotImplementedException();
        }

        public UserModel SearchSectionUser()
        {
            throw new NotImplementedException();
        }
    }
}
