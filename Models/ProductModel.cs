using MercadoSocial.Enums;
using System.ComponentModel.DataAnnotations;

namespace MercadoSocial.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha um nome para o produto.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Preencha uma seção para o produto.")]
        public SecaoEnum Section { get; set; }


        [Required(ErrorMessage = "Preencha uma descrição para o produto.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Preencha a quantidade do produto.")]
        public int Quantity { get; set; }

        public int? UserId { get; set; }
        public UserModel? User { get; set; }


        public byte[]? Image { get; set; }

        public string? ImageBase64
        {
            get
            {
                if (Image != null && Image.Length > 0)
                {
                    return Convert.ToBase64String(Image);
                }
                return null;
            }
        }
    }
}
