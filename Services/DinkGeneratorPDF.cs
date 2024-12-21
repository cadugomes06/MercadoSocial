using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices;

namespace MercadoSocial.Services
{
    public class DinkGeneratorPDF
    {
        private readonly IConverter _converter;
        public DinkGeneratorPDF(IConverter converter) 
        {
           _converter = converter;

            var dllPath = Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll");
            if (!File.Exists(dllPath))
            {
                throw new FileNotFoundException($"DLL não encontrada: {dllPath}");
            }
            NativeLibrary.Load(dllPath);
        }




        public byte[] GeneratePDF(string htmlContent)
        {

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 0, Bottom = 10, Left = 2, Right = 2 },
                DocumentTitle = "DinkPDF",                
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent, // Conteúdo principal do PDF
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = new FooterSettings
                {
                    FontSize = 12,
                    Line = true,
                    Right = "Página [page] de [toPage] - " + DateTime.Now.Year
                }
            };

            // Criar o documento
            var document = new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            // Converter para PDF
            return _converter.Convert(document);
        }




    }
}
