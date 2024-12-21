using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using MercadoSocial.Services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;

namespace MercadoSocial.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly InvoiceRenderingService _invoiceRenderingService;
        private readonly IProductRepositorio _productRepositoy;

        public InvoiceController(InvoiceRenderingService invoiceRenderingService, IProductRepositorio productRepository)
        {
            _invoiceRenderingService = invoiceRenderingService;
            _productRepositoy = productRepository;
        }

        public interface IComponent
        {
            void Compose(IContainer container);
        }


        public class HeaderDefault : IComponent
        {
         
          public void Compose(IContainer container)
            {
                container.PaddingVertical(20).Background(Colors.Grey.Medium).Column(column =>
                {
                    column.Item().Text("Header Component").AlignCenter().Bold();
                });
            }
        }


        public async Task<IActionResult> GeneratePDF()
        {

            try
            {
                ProductModel product = await _productRepositoy.GetProductById(7);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    var document = _invoiceRenderingService.GenerateInvoicePDF(product);
                    return File(document, "application/pdf", "exemplo.pdf");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
