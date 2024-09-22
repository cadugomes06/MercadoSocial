using System.ComponentModel.DataAnnotations;

namespace MercadoSocial.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Preencha um login válido")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Preencha uma senha válida")]
        public string Password { get; set; }
    }
}
