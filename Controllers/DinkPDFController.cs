using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using MercadoSocial.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Printing;


namespace MercadoSocial.Controllers
{
    public class DinkPDFController : Controller
    {
        private readonly DinkGeneratorPDF _dinkGeneratorPDF;
        private readonly IProductRepositorio _productRepositorio;
        public DinkPDFController(DinkGeneratorPDF dinkGeneratorPDF, IProductRepositorio productRepositorio)
        {
            _dinkGeneratorPDF = dinkGeneratorPDF;
            _productRepositorio = productRepositorio;
        }



        public async Task<ActionResult> GeneratePDF()
        {
            ProductModel product = await _productRepositorio.GetProductById(3);

            if (product == null)
            {
                throw new Exception("Não foi possível localizar o produto");
            }

            string imageBase64 = product.ImageBase64 ?? Convert.ToBase64String(product.Image);

            string htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
<style>
@media print {{
    .page-break {{
        page-break-before: always;
    }}

    .header {{
        display: block;
    }}

    .header-on-break {{
        display: none;
    }}
}}

.header-on-break {{
    display: none;
}}

.page-break + .header-on-break {{
    display: block;
}}

</style>
</head>
<body>
  
  <table style=""width:100%; height:160px; margin-bottom: 40px;"">
    <tr style=""text-align:center;"">
      <td style=""background:white;"" rowspan=""2"">
          <img src=""data:image/png;base64,{imageBase64}"" style=""width: 120px; height: 120px;"" />
      </td>    
      <td style=""background:teal;"">{product.Name}</td>      
      <td style=""background:teal; "">{product.Description}</td>    
    </tr>
    
    <tr style=""text-align:center;"">
      <td style=""background:purple;"">{product.Quantity}</td>
      <td style=""background:purple;"">{product.Section}</td>  
    </tr>
  </table>


<h3 style=""text-align:center;""><strong>Sumário</strong></h3>
<p><strong>1.0 - Testando primeiro Sumário</strong></p>
  <p>&nbsp; 1.1 - Sub categoria do sumário</p>
  <p>&nbsp; 1.2 - Segunda Sub categoria</p>
  <p><strong>2.0 - Segunda Categoria</strong></p>
  <p><strong>3.0 - Testando primeiro Sumário</strong></p>
  <p>&nbsp; 3.1 - Sub categoria do sumário</p>
  <p>&nbsp; 3.2 - Segunda Sub categoria</p>
  <p><strong>4.0 - Segunda Categoria</strong></p>
  <p><strong>5.0 - Testando primeiro Sumário</strong></p>
  <p>&nbsp; 5.1 - Sub categoria do sumário</p>
  <p>&nbsp; 5.2 - Segunda Sub categoria</p>  
  <p>&nbsp; 5.3 - Segunda Sub categoria</p>
  <p>&nbsp; 5.4 - Segunda Sub categoria</p>
  <p>&nbsp; 5.5 - Segunda Sub categoria</p>
  <p><strong>7.0 - Segunda Categoria</strong></p>

<div class=""page-break""></div>

  <p>&nbsp; 7.1 - Sub categoria do sumário</p>
  <p>&nbsp; 7.2 - Segunda Sub categoria</p>  
  <p>&nbsp; 7.3 - Segunda Sub categoria</p>
  <p>&nbsp; 7.4 - Segunda Sub categoria</p>
  <p>&nbsp; 7.5 - Segunda Sub categoria</p>  
  <p>&nbsp; 7.6 - Segunda Sub categoria</p>
  <p><strong>8.0 - Segunda Categoria</strong></p>
  <p><strong>9.0 - Segunda Categoria</strong></p>
  <p>&nbsp; 9.1 - Sub categoria do sumário</p>
  <p>&nbsp; 9.2 - Segunda Sub categoria</p>  
  <p>&nbsp; 9.3 - Segunda Sub categoria</p>
  <p>&nbsp; 9.4 - Segunda Sub categoria</p>
  <p>&nbsp; 9.5 - Segunda Sub categoria</p>  
  <p>&nbsp; 9.6 - Segunda Sub categoria</p>
  <p><strong>10.0 - Segunda Categoria</strong></p>

<div class=""page-break""></div>

  <p><strong>11.0 - Segunda Categoria</strong></p>
  <p>&nbsp; 11.1 - Sub categoria do sumário</p>
  <p>&nbsp; 11.2 - Segunda Sub categoria</p>  
  <p>&nbsp; 11.3 - Segunda Sub categoria</p>
  <p>&nbsp; 11.4 - Segunda Sub categoria</p>
  <p>&nbsp; 11.5 - Segunda Sub categoria</p>  
  <p>&nbsp; 11.6 - Segunda Sub categoria</p>
  <p><strong>12.0 - Segunda Categoria</strong></p>
  <p><strong>13.0 - Segunda Categoria</strong></p>
  <p>&nbsp; 13.1 - Sub categoria do sumário</p>
  <p>&nbsp; 13.2 - Segunda Sub categoria</p>  
  <p>&nbsp; 13.3 - Segunda Sub categoria</p>
  <p>&nbsp; 13.4 - Segunda Sub categoria</p>
  <p>&nbsp; 13.5 - Segunda Sub categoria</p>  
  <p>&nbsp; 13.6 - Segunda Sub categoria</p>
  <p><strong>14.0 - Segunda Categoria</strong></p>
  <p>&nbsp; 14.1 - Sub categoria do sumário</p>
  <p>&nbsp; 14.2 - Segunda Sub categoria</p>  
  <p>&nbsp; 14.3 - Segunda Sub categoria</p>
  <p>&nbsp; 14.4 - Segunda Sub categoria</p>
  <p>&nbsp; 14.5 - Segunda Sub categoria</p>  
  <p>&nbsp; 14.6 - Segunda Sub categoria</p>
  <p><strong>15.0 - Segunda Categoria</strong></p>
  <p><strong>16.0 - Segunda Categoria</strong></p>
  <p>&nbsp; 16.1 - Sub categoria do sumário</p>
  <p>&nbsp; 16.2 - Segunda Sub categoria</p>  
  <p>&nbsp; 16.3 - Segunda Sub categoria</p>
  <p>&nbsp; 16.4 - Segunda Sub categoria</p>
  <p>&nbsp; 16.5 - Segunda Sub categoria</p>  
  <p>&nbsp; 17.6 - Segunda Sub categoria</p>
  <p><strong>18.0 - Segunda Categoria</strong></p>

</body>
</html>
";

            byte[] pdfBytes = _dinkGeneratorPDF.GeneratePDF(htmlContent);

            return File(pdfBytes, "application/pdf", "dinkPdf.pdf");

        }





    }
}
