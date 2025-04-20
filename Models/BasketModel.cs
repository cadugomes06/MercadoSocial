namespace MercadoSocial.Models
{
    public class BasketModel
    {
        public int Id { get; set; }
        public ICollection<BasketItemsModel> Items { get; set; }
        public UserModel Criador { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
