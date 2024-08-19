using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class ClientInfo(string title, int subtitle, int columnSize, string cliente, string rut) : IComponent
    {
        public string SectionTitle { get; set; } = title;
        public int SubtitleSize { get; set; } = subtitle;
        public int FirstColumnSize { get; set; } = columnSize;
        public string Cliente { get; set; } = cliente;
        public string RUT { get; set; } = rut;

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(3);

                col.Item()
                   .PaddingBottom(5)
                   .Component(new SectionTitle(SectionTitle, SubtitleSize));

                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Cliente:", Cliente));
                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " RUT:", RUT));
            });
        }
    }
}
