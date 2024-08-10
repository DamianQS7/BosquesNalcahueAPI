using BosquesNalcahue.Application.Services;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class DetalleCarga(string title, string rowName, double alturaMedia, string medidaCamion, string bancos,
        double altoPalomera, string anchoPalomera) : IComponent
    {
        public string SectionTitle { get; set; } = title;
        public string RowName { get; set; } = rowName;
        public double AlturaMedia { get; set; } = alturaMedia;
        public string MedidaCamion { get; set; } = medidaCamion;
        public string Bancos { get; set; } = bancos;
        public string AnchoPalomera { get; set; } = anchoPalomera;
        public double AlturaMediaPalomera { get; set; } = altoPalomera;
        public string AlturaMediaPalomeraStr
        {
            get
            {
                if (AlturaMediaPalomera == 0)
                    return string.Empty;
                else
                    return AlturaMediaPalomera.ToString("F2");
            }
        }

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.Spacing(68);

                    // Camion Table
                    row.AutoItem().Table(table =>
                    {
                        // Table columns definition
                        table.ColumnsDefinition(col =>
                        {
                            col.ConstantColumn(140);
                            col.ConstantColumn(80);
                        });

                        // Table Header definition
                        table.Header(header =>
                        {
                            header.Cell().ColumnSpan(2).Element(PdfGeneratorService.SummaryHeaderCellStyle).Text(SectionTitle);
                        });

                        // Table Content
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text("H Media (m)");
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(AlturaMedia.ToString("F2"));

                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text($"{RowName} (m)");
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(MedidaCamion);

                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text("N° de Bancos");
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(Bancos);
                    });

                    // Palomera Table
                    row.AutoItem().Table(table =>
                    {
                        // Table columns definition
                        table.ColumnsDefinition(col =>
                        {
                            col.ConstantColumn(140);
                            col.ConstantColumn(80);
                        });

                        // Table Header definition
                        table.Header(header =>
                        {
                            header.Cell().ColumnSpan(2).Element(PdfGeneratorService.SummaryHeaderCellStyle).Text("Palomera");
                        });

                        // Table Content
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text("H Media (m)");
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(AlturaMediaPalomeraStr);

                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text("Ancho (m)");
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(AnchoPalomera);
                    });
                });
            });
        }
    }
}
