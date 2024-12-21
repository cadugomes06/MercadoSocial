

using DinkToPdf;
using DinkToPdf.Contracts;
using MercadoSocial.Data;
using MercadoSocial.Helper;
using MercadoSocial.Repositorio;
using MercadoSocial.Repositorio.Interfaces;
using MercadoSocial.Services;
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
            );

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<IProductRepositorio, ProductRepositorio>();
            builder.Services.AddScoped<IUserRepositorio, UserRepositorio>();
            builder.Services.AddScoped<ISessao, Sessao>();

            builder.Services.AddTransient<InvoiceRenderingService>();

            //builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            //builder.Services.AddScoped<DinkGeneratorPDF>();

            builder.Services.AddSession(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

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

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
