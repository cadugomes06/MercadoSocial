using MercadoSocial.Models;

namespace MercadoSocial.Helper
{
    public interface ISessao
    {
        void createSectionUser(UserModel user);
        void RemoveSectionUser();
        UserModel SearchSectionUser();
    }
}
