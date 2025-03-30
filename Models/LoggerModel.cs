namespace MercadoSocial.Models
{
    public class LoggerModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DataDoRegistro { get; set; } = DateTime.Now;
        public int? UserId { get ; set; }
        public virtual UserModel? User { get; set; }
    }
}
