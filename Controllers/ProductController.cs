using MercadoSocial.Enums;
using MercadoSocial.Filters;
using MercadoSocial.Helper;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace MercadoSocial.Controllers
{
    [PageToUserLogged]
    public class ProductController : Controller
    {

        private readonly IProductRepositorio _productRepositorio;
        private readonly ISessao _sessao;
        public ProductController(IProductRepositorio productRepositorio, ISessao sessao)
        {
            _productRepositorio = productRepositorio;
            _sessao = sessao;
        }


        public async Task<IActionResult> Index(List<ProductModel>? productsBySection)
        {
            try
            {
                if (productsBySection != null && productsBySection.Count() > 0)
                {
                    return View(productsBySection);
                }
                else
                {
                    List<ProductModel> products = await _productRepositorio.GetAllProducts();
                    return View(products);
                }
            }
            catch (Exception er)
            {
                TempData["ErroMensage"] = "Ooops.. Houve um erro ao redirecionar para essa página" + er.Message;
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
                    return Json(new {success = false, message = TempData});
                }
                return Json(productDB) ;
            }
            catch (Exception ex) 
            {
                TempData["ErroMessage"] = "Não foi possível localizar o produto." + ex.Message;
                return Json(new {success = false, message = TempData});
            }
            
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepositorio.GetProductById(id);
            if (product == null)
            {
                throw new Exception("Ops, houve um erro ao localizar o produto.");
            }

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
                        TempData["SuccessMensage"] = "Produtos localizados com sucesso";
                        return Json(products);
                    }
                }
                TempData["ErrorMessage"] = "Nenhum produto encontrado para a seção selecionada.";
                return Json(new List<object>());
            }
            catch (Exception er)
            {
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
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErroMessage"] = "Ops... Houve um erro ao criar o seu novo produto. " + ex.Message;
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
                    TempData["SuccessMessage"] = "Produto(s) adicionado com sucesso!";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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
                TempData["SuccessMessage"] = "Produto editado com sucesso.";
                return RedirectToAction("Index");
            
            }
            catch (Exception ex)
            {
                TempData["ErroMessage"] = "Ops... Houve um erro ao criar o seu novo produto. " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ProductModel>> RemoveProduct(int id)
        {
                bool productRemoved = await _productRepositorio.RemoveProduct(id);
                return Ok(productRemoved);
        }



    }
}