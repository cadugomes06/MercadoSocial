using MercadoSocial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoSocial.Data.Map
{
    public class LoggerMap : IEntityTypeConfiguration<LoggerModel>
    {
        public void Configure(EntityTypeBuilder<LoggerModel> builder)
        {
            builder.HasKey(x => x.Id);            
            builder.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId);
        }
    }
}
