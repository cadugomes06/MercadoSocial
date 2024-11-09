using MercadoSocial.Enums;
using MercadoSocial.Helper;

namespace MercadoSocial.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public PerfilEnum Perfil { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public virtual List<ProductModel> Products { get; set; }
        public bool PasswordIsValid(string password)
        {
            return Password == password.GenerateHash();
        }

        public void setPasswordHash()
        {
            Password = Password.GenerateHash();
        }

    }
}
