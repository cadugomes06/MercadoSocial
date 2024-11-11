using MercadoSocial.Data;
using MercadoSocial.Enums;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MercadoSocial.Repositorio
{
    public class ProductRepositorio : IProductRepositorio
    {
        private readonly BankDbContext _bankDbContext;
        public ProductRepositorio(BankDbContext dbContext) 
        {
            _bankDbContext = dbContext;
        }
        public async Task<List<ProductModel>> GetAllProducts()
        {
            List<ProductModel> products = await _bankDbContext.Produtos.ToListAsync();
            return products;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
           ProductModel product = await _bankDbContext.Produtos.FirstOrDefaultAsync(p => p.Id == id);
           return product;
        }        


        public async Task<List<ProductModel>> GetProductsBySection(Enum section)
        {
            if (section != null)
            {
                var products = await _bankDbContext.Produtos.Where(p => p.Section == (SecaoEnum)section)
               .ToListAsync();
                return (products);
            }
            return null;
        }

        public ProductModel CreateProduct(ProductModel product)
        {         
             _bankDbContext.Produtos.Add(product);
             _bankDbContext.SaveChanges();
             return product;
        }


        public async Task<ProductModel> AddProduct(int quantity, int id)
        {
            ProductModel productDb = await GetProductById(id); 
            if(productDb == null) {
                throw new Exception("Houve um erro inesperado, produto não encontrado!");
            }
            productDb.Quantity += quantity;
            _bankDbContext.Produtos.Update(productDb);
            await _bankDbContext.SaveChangesAsync();
            return productDb;
        }


        public async Task<ProductModel> EditProduct(ProductModel product)
        {
            ProductModel productDb = await GetProductById(product.Id);
            if (productDb == null) {
                throw new Exception("Houve um erro inesperado, produto não encontrado!");
            }
            productDb.Name = product.Name;
            productDb.Description = product.Description;
            productDb.Section = product.Section;
            if(product.Image != null)
            {
              productDb.Image = product.Image;
            }

            _bankDbContext.Update(productDb);
            _bankDbContext.SaveChanges();
            return productDb;                        
        }

        public async Task<bool> RemoveProduct(int id)
        {
            ProductModel productDB = await GetProductById(id);
            if(productDB == null)
            {
                throw new Exception("Houve um erro inesperado, produto não encontrado!");
            }

            _bankDbContext.Remove(productDB);
            await _bankDbContext.SaveChangesAsync();
            return true;
        }
    }


}
