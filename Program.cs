

using MercadoSocial.Data;
using MercadoSocial.Repositorio;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MercadoSocial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddEntityFrameworkSqlServer().AddDbContext<BankDbContext>
            (
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"))
            ); ;

            builder.Services.AddScoped<IProductRepositorio, ProductRepositorio>();
            builder.Services.AddScoped<IUserRepositorio, UserRepositorio>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
