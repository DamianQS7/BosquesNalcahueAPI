using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class DocumentHeader(string title, string imagePath, int titleSize, string folio, DateTime reportDate) : IComponent
    {
        public string Title { get; set; } = title;
        public string ImagePath { get; set; } = imagePath;
        public int TitleSize { get; set; } = titleSize;
        public string Folio { get; set; } = folio;
        public DateTime ReportDate { get; set; } = reportDate;

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                      .PaddingBottom(5)
                      .Row(row =>
                      {
                          row.ConstantItem(80)
                             .Image(ImagePath);

                          row.RelativeItem()
                              .AlignRight()
                              .PaddingTop(5)
                              .Column(column =>
                              {
                                  column.Spacing(5);

                                  column.Item()
                                        .Text("Detalle Despacho")
                                        .Bold()
                                        .FontSize(TitleSize);

                                  column.Item()
                                        .Text(Title);
                              });
                      });

                // N° & Fecha ColumnHeaders
                column.Item()
                      .PaddingRight(270)
                      .Row(row =>
                      {
                          row.ConstantItem(150)
                             .Border(1)
                             .AlignCenter()
                             .Text(x =>
                             {
                                 x.Span("N°")
                                  .Bold();
                             });

                          row.RelativeItem()
                             .Border(1)
                             .AlignCenter()
                             .Text(x =>
                             {
                                 x.Span("Fecha")
                                  .Bold();
                             });
                      });

                // N° y Fecha actual values
                column.Item()
                      .PaddingRight(270)
                      .Row(row =>
                      {
                          row.ConstantItem(150)
                             .Border(1)
                             .AlignCenter()
                             .Text(Folio);

                          row.RelativeItem()
                             .Border(1)
                             .AlignCenter()
                             .Text(ReportDate.ToString());
                      });
            });
        }
    }
}
