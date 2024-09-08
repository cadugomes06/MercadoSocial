using MercadoSocial.Models;

namespace MercadoSocial.Repositorio.Interfaces
{
    public interface IProductRepositorio
    {
        Task<List<ProductModel>> GetAllProducts();        
        Task<ProductModel> GetProductById(int id);
        Task<List<ProductModel>> GetProductsBySection(Enum section);
        ProductModel CreateProduct(ProductModel product);
        Task<ProductModel> AddProduct(int quantity, int id);
        Task<ProductModel> EditProduct(ProductModel product);
    }
}
