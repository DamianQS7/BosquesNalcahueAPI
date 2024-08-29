using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.PdfComponents;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.Services;

public class PdfGeneratorService(string basePath) : IPdfGeneratorService
{
    #region Properties

    public int TitleSize { get; set; } = 18;
    public int SubtitleSize { get; set; } = 14;
    public int NormalSize { get; set; } = 12;
    public int FooterSize { get; set; } = 10;
    public int FirstColumnSize { get; set; } = 140;
    public string ImagePath { get; set; } = Path.Combine(basePath, "pdf_image.png");

    #endregion

    #region Methods

    public IDocument CreateLenaReport(SingleProductReport model)
    {
        // Design the PDF
        return
        Document.Create(container =>
        {
            container.Page(page =>
            {
                // Page Settings
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(NormalSize));

                // Document Sections
                page.Header()
                    .Component(new DocumentHeader("Leña", ImagePath, TitleSize, model.Folio!, model.Date));

                page.Content()
                    .PaddingVertical(0, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Height(0);

                        //Datos Cliente
                        x.Item()
                         .Component(
                            new ClientInfo("Datos Cliente", SubtitleSize, FirstColumnSize, model.ClientName ?? "", model.ClientId ?? ""));

                        // Datos Camión
                        x.Item()
                         .Component(
                            new CamionInfo("Datos Camión", SubtitleSize, FirstColumnSize, model.TruckPlate ?? "",
                            model.TruckDriver ?? "", model.TruckDriverId ?? "", model.TruckCompany ?? ""));

                        //// Despacho Leña
                        x.Item()
                         .Component(
                            new DespachoInfo("Despacho Leña", SubtitleSize, FirstColumnSize, model.ProductName ?? "", model.Origin ?? ""));

                        // Detalles de Carga
                        x.Item().Column(col =>
                        {
                            col.Spacing(3);

                            col.Item()
                               .Component(new SectionTitle("Detalles de Carga", SubtitleSize));

                            col.Item()
                               .Component(
                                new DetalleCarga("Camión", "Largo Camión", model.TruckHeight, model.TruckLength.ToString(),
                                model.Banks.ToString(), model.PalomeraHeight, model.PalomeraWidth.ToString()));


                            col.Item().Height(12);
                            col.Item().Table(table =>
                            {
                                // Table columns definition
                                table.ColumnsDefinition(col =>
                                {
                                    col.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(SummaryHeaderCellStyle).Text("Resultado Cantidad de Leña");
                                });

                                table.Cell().Element(SummaryFinalCellStyle).Text($"{model.FinalQuantity:F2} ML");
                            });
                        });

                    });

                page.Footer()
                    .Element(ComposeFooter);
            });
        });
    }

    public IDocument CreateMetroRumaReport(SingleProductReport model)
    {
        // Design the PDF
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                // Page Settings
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(NormalSize));

                // Document Sections
                page.Header()
                    .Component(new DocumentHeader("Metro Ruma", ImagePath, TitleSize, model.Folio!, model.Date));

                page.Content()
                    .PaddingVertical(0, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Height(0);

                        //Datos Cliente
                        x.Item()
                         .Component(
                            new ClientInfo("Datos Cliente", SubtitleSize, FirstColumnSize, model.ClientName ?? "", model.ClientId ?? ""));

                        // Datos Camión
                        x.Item()
                         .Component(new CamionInfo("Datos Camión", SubtitleSize, FirstColumnSize, model.TruckPlate ?? "",
                         model.TruckDriver ?? "", model.TruckDriverId ?? "", model.TruckCompany ?? ""));

                        // Despacho Leña
                        x.Item().Component(new DespachoInfo("Despacho Metro Ruma", SubtitleSize, FirstColumnSize,
                            model.ProductName ?? "", model.Origin ?? ""));

                        // Detalles de Carga
                        x.Item().Column(col =>
                        {
                            col.Spacing(3);

                            col.Item()
                               .Component(new SectionTitle("Detalles de Carga", SubtitleSize));

                            col.Item()
                               .Component(new DetalleCarga("Camión y Carro", "Ancho Camión", model.TruckHeight, model.TruckLength.ToString(),
                               model.Banks.ToString(), model.PalomeraHeight, model.PalomeraWidth.ToString()));


                            col.Item().Height(12);
                            col.Item().Table(table =>
                            {
                                // Table columns definition
                                table.ColumnsDefinition(col =>
                                {
                                    col.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(SummaryHeaderCellStyle).Text("Resultado Cantidad de Metro Ruma");
                                });

                                table.Cell().Element(SummaryFinalCellStyle).Text($"{model.FinalQuantity:F2} MR");
                            });
                        });

                    });

                page.Footer()
                    .Element(ComposeFooter);
            });
        });
    }

    public IDocument CreateTrozoAserrableReport(MultiProductReport model)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                #region Page Settings

                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(NormalSize));

                #endregion

                page.Header()
                    .Component(new DocumentHeader("Venta en Trozos", ImagePath, TitleSize, model.Folio!, model.Date));

                page.Content()
                    .PaddingVertical(0, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Height(0);

                        //Datos Cliente
                        x.Item()
                         .Component(
                            new ClientInfo("Datos Cliente", SubtitleSize, FirstColumnSize, model.ClientName ?? "", model.ClientId ?? ""));

                        // Datos Camión
                        x.Item()
                         .Component(
                            new CamionInfoTrozo("Datos Camión", SubtitleSize, FirstColumnSize, model.TruckPlate ?? "",
                         model.TruckDriver ?? "", model.TruckDriverId ?? "", model.TruckCompany ?? ""));

                        // Detalles de Carga

                        // Lista 1

                        if (model.Products?.Count > 0)
                        {
                            int total = model.Products.Count;
                            int count = 0;
                            while (count < total)
                            {
                                if (count > 0)
                                    x.Item().Height(0).PageBreak();

                                x.Item().Component(
                                    new EspecieInfo($"Producto {count + 1}", SubtitleSize, FirstColumnSize, model.Products[count].Species,
                                    model.Products[count].Length.ToString(), model.Products[count].Origin));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga:", 13));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.Products[count].Measurements ?? [],
                                    model.Products[count].QuantitySum, model.Products[count].VolumeSum));

                                count++;
                            }
                        }

                        // Summary
                        x.Item().Column(col =>
                        {
                            // Title
                            col.Item()
                               .Component(new SectionTitle("Resumen", SubtitleSize));

                            //Table
                            col.Item().Component(new TrozoAserrableSummary(model));
                        });

                    });

                page.Footer()
                    .Element(ComposeFooter);
            });
        });
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Static method that formats the name of the file to be generated.
    /// </summary>
    /// <param name="nombreCliente">String that represents the Nombre property in the Cliente object of the viewModel</param>
    /// <param name="userInitals">String that represents the initials of the user generating the PDF.</param>
    /// <returns></returns>
    public static string GenerateFileName(string nombreCliente, string userInitials, int fileNumber)
    {
        // Get the current date
        string dateAsString = DateTime.Now.ToString("dd/MM/yy");
        string formattedDate = dateAsString.Replace("/", "-");

        // Get cliente's name
        string formattedCliente;
        if (string.IsNullOrEmpty(nombreCliente))
            formattedCliente = "Sin_Nombre";
        else
            formattedCliente = nombreCliente.Trim().Split(' ')[0];

        // Return the formatted string
        return $"{fileNumber}{userInitials}{formattedDate}_{formattedCliente}.pdf";
    }


    /// <summary>
    /// Static method that defines the style for the header cells of a table.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static IContainer HeaderCellStyle(IContainer container)
    {
        return container.DefaultTextStyle(x => x.SemiBold())
                        .PaddingVertical(5)
                        .BorderBottom(1)
                        .AlignCenter()
                        .BorderColor(Colors.Black);
    }

    /// <summary>
    /// Static method that defines the style for the cells of a table.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1)
                        .AlignCenter()
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingVertical(5);
    }

    public static IContainer SummaryCellStyle(IContainer container)
    {
        return container.Border(1)
                        .AlignCenter()
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingVertical(5);
    }

    public static IContainer SummaryFinalCellStyle(IContainer container)
    {
        return container.Border(1)
                        .AlignCenter()
                        .DefaultTextStyle(x => x.FontSize(18))
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingVertical(5);
    }

    public static IContainer SummaryHeaderCellStyle(IContainer container)
    {
        return container.DefaultTextStyle(x => x.SemiBold())
                        .PaddingTop(5)
                        .Border(1)
                        .AlignCenter()
                        .BorderColor(Colors.Black);
    }

    /// <summary>
    /// Method that defines the layout for the footer of the PDF.
    /// </summary>
    /// <param name="container"></param>
    private static void ComposeFooter(IContainer container)
    {
        container
                .AlignCenter()
                .DefaultTextStyle(x => x.FontSize(10))
                .Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                });
    }

    #endregion
}
