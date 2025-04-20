using MercadoSocial.Enums;
using MercadoSocial.Filters;
using MercadoSocial.Helper;
using MercadoSocial.Logger;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;


namespace MercadoSocial.Controllers
{
    [PageToUserLogged]
    public class ProductController : Controller
    {

        private readonly IProductRepositorio _productRepositorio;
        private readonly ISessao _sessao;
        private readonly ILoggerService _logger;

        public ProductController(IProductRepositorio productRepositorio, ISessao sessao, ILoggerService logger)
        {
            _productRepositorio = productRepositorio;
            _sessao = sessao;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                await _logger.CreateLogger("Entrou na pagina de Produtos", $"Usuario acessou a pagina de Produtos", _sessao.SearchSectionUser().Id);
                List<ProductModel> products = await _productRepositorio.GetAllProducts();
                return View(products);

            }
            catch (Exception er)
            {
                TempData["ErroMensage"] = "Ooops.. Houve um erro ao redirecionar para essa página" + er.Message;
                await _logger.CreateLogger("Entrando na Home", $"Houve uma falha ao redirecionar para a Home, {er.Message}", _sessao.SearchSectionUser().Id);
                return View();
            }
        }


        public IActionResult Create()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }



        public async Task<JsonResult> GetProductById(int id)
        {
            try
            {
                ProductModel productDB = await _productRepositorio.GetProductById(id);
                if (productDB == null)
                {
                    TempData["ErroMessage"] = "Houve um erro ao localizar o produto.";
                    await _logger.CreateLogger("Buscando um produto", "Produto não localizado", _sessao.SearchSectionUser().Id);
                    return Json(new { success = false, message = TempData });
                }

                await _logger.CreateLogger("Buscando um produto", $"Produto: {productDB.Name} encontrado!", _sessao.SearchSectionUser().Id);
                return Json(productDB);
            }
            catch (Exception ex)
            {
                TempData["ErroMessage"] = "Não foi possível localizar o produto." + ex.Message;
                await _logger.CreateLogger("Buscando um produto", $"Houve uma falha interna ao tentar localizar o produto: {ex.Message}", _sessao.SearchSectionUser().Id);
                return Json(new { success = false, message = TempData });
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepositorio.GetProductById(id);
            if (product == null)
            {
                await _logger.CreateLogger("Editando um Produto", "Produto não localizado para edição", _sessao.SearchSectionUser().Id);
                throw new Exception("Ops, houve um erro ao localizar o produto.");
            }

            await _logger.CreateLogger("Editando um Produto", $"Produto: {product.Name}, localizado para edição", _sessao.SearchSectionUser().Id);
            return View(product);
        }



        [HttpGet]
        public async Task<IActionResult> GetProductsBySection(string section)
        {
            try
            {

                if (Enum.TryParse(typeof(SecaoEnum), section, true, out var sectionEnum))
                {
                    var products = await _productRepositorio.GetProductsBySection((SecaoEnum)sectionEnum);
                    if (products != null && products.Any())
                    {
                        await _logger.CreateLogger("Filtrando produtos por sessão", "Produto filtrados foram localizado", _sessao.SearchSectionUser().Id);
                        TempData["SuccessMensage"] = "Produtos localizados com sucesso";
                        return Json(products);
                    }
                }

                await _logger.CreateLogger("Filtrando produtos por sessão", "Produto filtrados foram localizado", _sessao.SearchSectionUser().Id);
                TempData["ErrorMessage"] = "Nenhum produto encontrado para a seção selecionada.";
                return Json(new List<object>());
            }
            catch (Exception er)
            {
                await _logger.CreateLogger("Filtrando produtos por sessão", $"Houve um erro interno ao tentar filtrar por sessão: {er.Message}", _sessao.SearchSectionUser().Id);
                TempData["ErrorMessage"] = "Houve um erro na busca: " + er.Message;
                return Json(new { error = true, message = "Houve um erro na busca: " + er.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create(ProductModel product, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);
                            product.Image = memoryStream.ToArray();
                        }
                    }

                    UserModel userLogado = _sessao.SearchSectionUser();
                    product.UserId = userLogado.Id;

                    _productRepositorio.CreateProduct(product);
                    TempData["SuccessMessage"] = "Produto criado com sucesso.";
                    await _logger.CreateLogger("Criando Produto", $"Produto: {product.Name}, foi criado com sucesso!", _sessao.SearchSectionUser().Id);
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErroMessage"] = "Ops... Houve um erro ao criar o seu novo produto. " + ex.Message;
                await _logger.CreateLogger("Criando Produto", $"Houve um erro interno ao tentar criar o produto: {ex.Message}", _sessao.SearchSectionUser().Id);
                return RedirectToAction("Index");
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddProducts(int quantity, int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            try
            {
                var newProduct = await _productRepositorio.AddProduct(quantity, id);
                if (newProduct != null)
                {
                    await _logger.CreateLogger("Adicionando produto", $"Produto, {newProduct.Name}, adicionado com sucesso", _sessao.SearchSectionUser().Id);
                    TempData["SuccessMessage"] = "Produto(s) adicionado com sucesso!";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await _logger.CreateLogger("Adicionando produto", $"Houve um erro interno ao tentar adicionar o produto, {ex.Message}", _sessao.SearchSectionUser().Id);
                ModelState.AddModelError(string.Empty, "Um erro ocorreu: " + ex.Message);
                return View("Error");
            }
        }



        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel product, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        product.Image = memoryStream.ToArray();
                    }
                }

                await _productRepositorio.EditProduct(product);
                await _logger.CreateLogger("Produto editado", $"Produto, {product.Name}, foi editado com sucesso", _sessao.SearchSectionUser().Id);
                TempData["SuccessMessage"] = "Produto editado com sucesso.";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                await _logger.CreateLogger("Adicionando produto", $"Houve um erro interno ao tentar editar o produto, {ex.Message}", _sessao.SearchSectionUser().Id);
                TempData["ErroMessage"] = "Ops... Houve um erro ao criar o seu novo produto. " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<ActionResult<bool>> RemoveProduct(int id)
        {
            try
            {
                bool productRemoved = await _productRepositorio.RemoveProduct(id);
                if (productRemoved)
                {
                    await _logger.CreateLogger("Excluindo Produto", $"O Produto #{id} foi removido com sucesso", _sessao.SearchSectionUser().Id);
                    return Ok(productRemoved);
                }

                await _logger.CreateLogger("Excluindo Produto", $"Produto não foi encontrado para exclusão", _sessao.SearchSectionUser().Id);
                return BadRequest("Produto não encontrado ou não pode ser excluído.");
                ;
            }
            catch (Exception ex)
            {
                await _logger.CreateLogger("Excluindo Produto", $"Houve um erro interno ao tentar excluir o produto, {ex.Message}", _sessao.SearchSectionUser().Id);
                return StatusCode(500, "Erro interno ao tentar excluir o produto");
            }
        }


        [HttpGet]
        public IActionResult TypeSectionEnum(int section)
        {
            if (Enum.IsDefined(typeof(SecaoEnum), section))
            {
                var sectionEnum = (SecaoEnum)section;
                return Ok(sectionEnum.ToString()); // Nome do enum
            }
            return NotFound("Seção inválida.");
        }






    }
}