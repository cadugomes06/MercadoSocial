using MercadoSocial.Data.Map;
using MercadoSocial.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoSocial.Data
{
    public class BankDbContext : DbContext
    {

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) 
        {

        }

        public DbSet<ProductModel> Produtos {  get; set; }
        public DbSet<UserModel> Usuarios { get; set; }
        public DbSet<LoggerModel> Logger { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMap());
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LoggerMap());
            base.OnModelCreating(modelBuilder);
        }

    }
}
