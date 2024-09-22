using MercadoSocial.Enums;
using System.ComponentModel.DataAnnotations;

namespace MercadoSocial.Models
{
    public class UserSemSenhaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha um nome")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Preencha um email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Preencha um Login")]
        public string Login { get; set; }


        [Required(ErrorMessage = "Preencha um perfil")]
        public PerfilEnum Perfil { get; set; }
    }
}
