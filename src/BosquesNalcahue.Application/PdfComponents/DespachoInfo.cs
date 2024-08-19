using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class DespachoInfo(string title, int subtitle, int columnSize, string tipoLena, string origen) : IComponent
    {
        public string SectionTitle { get; set; } = title;
        public int SubtitleSize { get; set; } = subtitle;
        public int FirstColumnSize { get; set; } = columnSize;
        public string TipoLena { get; set; } = tipoLena;
        public string Origen { get; set; } = origen;

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(3);

                col.Item()
                   .Component(new SectionTitle(SectionTitle, SubtitleSize));

                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Tipo Leña:", TipoLena));

                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Unidad de Origen", Origen));
            });
        }
    }
}
