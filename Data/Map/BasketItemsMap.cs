using MercadoSocial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace MercadoSocial.Data.Map
{
    public class BasketItemsMap : IEntityTypeConfiguration<BasketItemsModel>
    {
        public void Configure(EntityTypeBuilder<BasketItemsModel> builder)
        {
            builder.HasOne(ba => ba.Basket)
                   .WithMany(b => b.Items)
                   .HasForeignKey(ci => ci.BasketId);
        }
    }
}
