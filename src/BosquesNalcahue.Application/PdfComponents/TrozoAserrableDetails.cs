using BosquesNalcahue.Application.Models;
using BosquesNalcahue.Application.Services;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class TrozoAserrableDetails(List<ProductMeasurement> medidas, int totalSum, double finalTotalSum) : IComponent
    {
        public List<ProductMeasurement> Medidas { get; set; } = medidas;
        public int TotalSum { get; set; } = totalSum;
        public double FinalTotalSum { get; set; } = finalTotalSum;

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                // Table columns definition
                table.ColumnsDefinition(col =>
                {
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                });

                // Table Header definition
                table.Header(header =>
                {
                    header.Cell().Element(PdfGeneratorService.HeaderCellStyle).Text("Diámetro");
                    header.Cell().Element(PdfGeneratorService.HeaderCellStyle).Text("Total (Q)");
                    header.Cell().Element(PdfGeneratorService.HeaderCellStyle).Text("Volumen (V)");
                    header.Cell().Element(PdfGeneratorService.HeaderCellStyle).Text("Total (Q x V)");
                });

                // Aqui agregar un foreach que recorra la lista de productos
                foreach (var item in Medidas)
                {
                    table.Cell().Element(PdfGeneratorService.CellStyle).Text(item.Diameter.ToString());
                    table.Cell().Element(PdfGeneratorService.CellStyle).Text(item.Quantity.ToString());
                    table.Cell().Element(PdfGeneratorService.CellStyle).Text(item.Volume.ToString("F2"));
                    table.Cell().Element(PdfGeneratorService.CellStyle).Text(item.Total.ToString());
                }

                // Last row to display totals
                table.Cell().Element(PdfGeneratorService.CellStyle).Text("Total:");
                table.Cell().Element(PdfGeneratorService.CellStyle).Text(TotalSum.ToString());
                table.Cell().Element(PdfGeneratorService.CellStyle).Text("");
                table.Cell().Element(PdfGeneratorService.CellStyle).Text(FinalTotalSum.ToString("F2"));
            });
        }
    }
}
