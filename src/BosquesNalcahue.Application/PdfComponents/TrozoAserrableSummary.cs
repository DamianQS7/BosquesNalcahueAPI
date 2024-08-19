using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Services;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class TrozoAserrableSummary(MultiProductReport model) : IComponent
    {
        public MultiProductReport ViewModel { get; set; } = model;

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                // Table columns definition
                table.ColumnsDefinition(col =>
                {
                    col.ConstantColumn(240);
                    col.RelativeColumn();
                    col.RelativeColumn();
                });

                // Table Header definition
                table.Header(header =>
                {
                    header.Cell().Element(PdfGeneratorService.SummaryHeaderCellStyle).Text("Producto");
                    header.Cell().Element(PdfGeneratorService.SummaryHeaderCellStyle).Text("Cantidad de Trozos");
                    header.Cell().Element(PdfGeneratorService.SummaryHeaderCellStyle).Text("Volumen (m3)");
                });

                // Table Content based on the number of products

                if (ViewModel.Products?.Count > 0)
                {
                    int total = ViewModel.Products.Count;
                    int count = 0;

                    while (count < total)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.Products[count].Species} {ViewModel.Products[count].Length} m.";

                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(producto);
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(ViewModel.Products[count].QuantitySum.ToString());
                        table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(ViewModel.Products[count].VolumeSum.ToString("F2"));

                        count++;
                    }
                }

                table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text("Total:");
                table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(ViewModel.FinalQuantity.ToString());
                table.Cell().Element(PdfGeneratorService.SummaryCellStyle).Text(ViewModel.FinalVolume.ToString("F2"));
            });
        }
    }
}
