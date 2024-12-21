using MercadoSocial.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.RegularExpressions;
using QuestPDF.Companion;
using IContainer = QuestPDF.Infrastructure.IContainer;
using IComponent = QuestPDF.Infrastructure.IComponent;


namespace MercadoSocial.Services
{
    public class InvoiceRenderingService 
    {
        public InvoiceRenderingService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }



        public class HeaderDefault : IComponent
        {

            public void Compose(IContainer container)
            {
                container.PaddingVertical(20).Background(Colors.Grey.Medium).Column(column =>
                {
                    column.Item().Text("HEADER DINÂMICO").AlignCenter().Bold();
                });
            }
        }



        public class FooterDefault : IComponent
        {

            public void Compose(IContainer container)
            {
                container.PaddingVertical(20).Background(Colors.Grey.Medium).Column(column =>
                {
                    column.Item().Text("Footer DINÂMICO").AlignCenter().Bold();
                });
            }
        }



        public byte[] GenerateInvoicePDF(ProductModel product)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    byte[] imageInBytes = string.IsNullOrWhiteSpace(product.ImageBase64)
                           ? null
                           : Convert.FromBase64String(product.ImageBase64);


                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            if (imageInBytes != null)
                            {
                                column.Item().AlignCenter().Width(150).Height(150).Image(imageInBytes);
                            }
                            else
                            {
                                column.Item().Text("Imagem indisponível").SemiBold();
                            }
                            column.Item().PaddingHorizontal(4).PaddingTop(20).AlignCenter().Text("PCMSO - PROGRAMA DE CONTRLE MÉDICO DE SAÚDE OCUPACIONAL").Bold().FontSize(20);
                        });
                    });



                    //CONTEÚDO
                    page.Content().PaddingTop(30).AlignCenter().Column(column =>
                    {
                        column.Item().AlignCenter().Text(text =>
                        {
                            text.Span("Produto: ").Bold().FontSize(18);
                            text.Span(product.Name).SemiBold().FontSize(16);
                        });

                        column.Item().AlignCenter().PaddingBottom(20).Text(product.Description).FontColor(Colors.Blue.Darken4).FontSize(16);

                        column.Item().Row(row =>
                        {
                            if (imageInBytes != null)
                            {
                                column.Item().AlignCenter().Width(120).Height(120).Image(imageInBytes);
                            }
                            else
                            {
                                column.Item().Text("Imagem indisponível").SemiBold();
                            }
                        });


                        column.Item().AlignCenter().PaddingTop(60).PaddingHorizontal(10).Table(table =>
                        {
                            table.ColumnsDefinition(columnTable =>
                            {
                                columnTable.ConstantColumn(100);
                                columnTable.ConstantColumn(100);
                                columnTable.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Accent1).Border(1).AlignCenter().PaddingVertical(2).Text("REVISÃO").FontColor("#ffffff").Bold();
                                header.Cell().Background(Colors.Blue.Accent2).Border(1).AlignCenter().PaddingVertical(2).Text("DATA").FontColor("#ffffff").Bold();
                                header.Cell().Background(Colors.Blue.Accent3).Border(1).AlignCenter().PaddingVertical(2).Text("DESCRIÇÃO").FontColor("#ffffff").Bold();
                            });

                            for (var i = 0; i < product.Quantity; i++)
                            {
                                var background = i % 2 == 0 ? Colors.White : Colors.Grey.Darken2;

                                table.Cell().Background(background).Border(1).BorderColor(Colors.Black.Blue).AlignCenter().Padding(4).Text(product.Quantity);
                                table.Cell().Background(background).Border(1).BorderColor(Colors.Black.Blue).AlignCenter().Padding(4).Text(product.Name);
                                table.Cell().Background(background).Border(1).BorderColor(Colors.Black.Blue).AlignCenter().Padding(4).Text(product.Description);

                            }

                        });
                    });

                    page.Footer().Text("FOCUS HEALTH SOLUTIONS SOLUÇÕES DE SAÚDE LTDA.").AlignCenter();
                });



                /* SUMÁRIO  */
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);

                    string sumarioHtmlAsString = "c";
                    List<string> sumarioClean = ExtractTextFromHtmlToArray(sumarioHtmlAsString);


                    page.Header().AlignCenter().Text("Página 2").FontSize(16).Bold();

                    page.Content().Column(column =>
                    {
                        column.Item().AlignCenter().PaddingVertical(20).Text("Sumário").FontSize(14).Bold();

                        for (int i = 0; i < sumarioClean.Count; i++)
                        {
                            bool verificartitulo = sumarioClean[i].Substring(0, 4).Contains(".0");

                            column.Item().Row(row =>
                            {
                                if (verificartitulo)
                                {
                                    row.AutoItem().PaddingTop(4).Text(sumarioClean[i]).Bold();
                                }
                                else
                                {
                                    row.AutoItem().PaddingLeft(4).Text(sumarioClean[i]);
                                }
                                //row.relativeitem().width(300).paddingvertical(2).borderbottom(1).bordercolor(colors.black.blue);
                            });
                        }

                    });

                    page.Footer().Text("Rodapé").AlignRight();
                });


                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);

                    page.Header().Component(new HeaderDefault());

                    page.Content().AlignCenter().PaddingTop(20).Table(table =>
                    {
                        table.ColumnsDefinition(columnTable =>
                        {
                            columnTable.ConstantColumn(80);
                            columnTable.ConstantColumn(80);
                        });

                        table.Header(header =>
                        {
                            header.Cell().RowSpan(2).Background(Colors.Blue.Accent1).Border(1).AlignCenter().PaddingVertical(2).Text("Empresa").FontColor("#ffffff").Bold();
                            header.Cell().RowSpan(2).Background(Colors.Blue.Accent1).Border(1).AlignCenter().PaddingVertical(2).Text("Endereço").FontColor("#ffffff").Bold();
                        });

                        table.Cell().ColumnSpan(2).Background(Colors.White).Border(1).AlignCenter().Text("FOCUS HEALTH SOLUTIONS");
                        table.Cell().ColumnSpan(2).Background(Colors.White).Border(1).AlignCenter().Text("Novo Endereço das Gaivotas");
                    });

                    page.Footer().Component(new FooterDefault());
                });

            });
            document.ShowInCompanion();

            return document.GeneratePdf();
        }







        public static List<string> ExtractTextFromHtmlToArray(string html)
        {
            if (string.IsNullOrEmpty(html))
                return new List<string>();

            // Extrai o conteúdo entre as tags HTML e coloca em um array
            var matches = Regex.Matches(html, @"<[^>]*>(.*?)<\/[^>]*>");
            var rawTexts = matches.Cast<Match>()
                                  .Select(match => match.Groups[1].Value)
                                  .ToList();

            // Limpa cada elemento do array (remove entidades HTML, como &nbsp;)
            var cleanTexts = rawTexts.Select(text =>
            {
                text = Regex.Replace(text, "<.*?>", string.Empty);
                text = Regex.Replace(text, @"&nbsp;", " ", RegexOptions.IgnoreCase); // Substitui &nbsp;
                text = Regex.Replace(text, @"\s+", " "); // Remove espaços extras
                return text.Trim(); // Remove espaços nas extremidades
            }).ToList();

            return cleanTexts;
        }







    }
}
