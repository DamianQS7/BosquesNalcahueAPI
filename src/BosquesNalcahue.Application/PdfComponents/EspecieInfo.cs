using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class EspecieInfo(string title, int subtitle, int columnSize, string especie, string largo, string origen) : IComponent
    {
        public string SectionTitle { get; set; } = title;
        public int SubtitleSize { get; set; } = subtitle;
        public int FirstColumnSize { get; set; } = columnSize;
        public string Especie { get; set; } = especie;
        public string Largo { get; set; } = largo;
        public string UnidadOrigen { get; set; } = origen;

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(3);

                col.Item()
                   .PaddingBottom(5)
                   .Component(new SectionTitle(SectionTitle, SubtitleSize));
                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Unidad de origen:", UnidadOrigen));
                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Producto:", Especie));
                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Largo Cubicación:", Largo));
            });
        }
    }
}
