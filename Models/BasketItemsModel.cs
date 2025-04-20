namespace MercadoSocial.Models
{
    public class BasketItemsModel
    {
        public int Id { get; set; }

        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        public virtual BasketModel Basket { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
