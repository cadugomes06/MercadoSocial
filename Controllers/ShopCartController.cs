using MercadoSocial.Helper;
using MercadoSocial.Logger;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercadoSocial.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IProductRepositorio _productRepositorio;
        private readonly ILoggerService _logger;
        private readonly ISessao _sessao;
        public ShopCartController(IProductRepositorio productRepositorio, ISessao sessao, ILoggerService logger)
        {
            _sessao = sessao;
            _logger = logger;
            _productRepositorio = productRepositorio; 
        }


        [HttpGet]
        public async Task<IActionResult> Index(List<int> productsIds)
        {
            if (productsIds == null || !productsIds.Any())
            {
                return BadRequest("Nenhum produto foi passado.");
            }

            string cartItemsJson = HttpContext.Session.GetString("CartItems");
            if (!string.IsNullOrEmpty(cartItemsJson) && cartItemsJson != "[]")
            {
                List<ProductModel> productsFromSession = await GetSessionFromCartItems(cartItemsJson);

                if (productsFromSession != null)
                {
                    return View(productsFromSession);
                }
            }

            try
            {
                var products = await _productRepositorio.GetProductsByIds(productsIds);
                if (products == null || !products.Any())
                {
                    return NotFound("Não foi encontrado nenhum produto!");
                }

                await _logger.CreateLogger("Carrinho de compras", "Buscando Produtos no carrinho de compras", _sessao.SearchSectionUser().Id);
                return View(products);
            }
            catch (Exception er)
            {
                await _logger.CreateLogger("Carrinho de compras", $"Falha interna ao buscar produtos no carrinho de compras: {er.Message}", _sessao.SearchSectionUser().Id);
                return StatusCode(500, $"Houve um erro interno ao tentar processar a requisição: {er.Message}");
            }
        }

        public async Task<List<ProductModel>> GetSessionFromCartItems(string cartItemsJson)
        {
            List<int> productsIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(cartItemsJson);

            if (productsIds == null || !productsIds.Any()) return null;

            List<ProductModel> products = new List<ProductModel>();
            products = await _productRepositorio.GetProductsByIds(productsIds);


            return products;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateListShopCart([FromBody] List<int> productsIds)
        {
            if (productsIds == null)
            {
                return BadRequest("Nenhum produto foi passado.");
            }

            try
            {
                List<ProductModel> products = await _productRepositorio.GetProductsByIds(productsIds);
                if (products == null)
                {
                    return NotFound("Não foi encontrado nenhum produto!");
                }

                HttpContext.Session.SetString("CartItems", System.Text.Json.JsonSerializer.Serialize(productsIds));

                await _logger.CreateLogger("Carrinho de compras", "Atualizando Produtos no carrinho de compras", _sessao.SearchSectionUser().Id);
                return PartialView("_ShopCartPartial", products);
            }
            catch (Exception er)
            {
                await _logger.CreateLogger("Carrinho de compras", $"Falha interna ao atualizar os produtos no carrinho de compras: {er.Message}", _sessao.SearchSectionUser().Id);
                return StatusCode(500, $"Houve um erro interno ao tentar processar a requisição: {er.Message}");
            }
        }


    }
}
