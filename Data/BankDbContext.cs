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

    }
}
