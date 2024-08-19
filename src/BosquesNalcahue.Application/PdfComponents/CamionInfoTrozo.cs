using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class CamionInfoTrozo(string title, int subtitle, int columnSize, string patente, string chofer, string rut, string empresa) : IComponent
    {
        public string SectionTitle { get; set; } = title;
        public int SubtitleSize { get; set; } = subtitle;
        public int FirstColumnSize { get; set; } = columnSize;
        public string Patente { get; set; } = patente;
        public string Chofer { get; set; } = chofer;
        public string RutChofer { get; set; } = rut;
        public string EmpresaTransportista { get; set; } = empresa;

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(3);

                col.Item()
                   .PaddingBottom(5)
                   .Component(new SectionTitle(SectionTitle, SubtitleSize));

                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Empresa de Transporte:",
                       EmpresaTransportista));

                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Chofer:", Chofer));

                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " RUT:", RutChofer));

                col.Item()
                   .Component(new SingleRow(FirstColumnSize, " Patentes", Patente));
            });
        }
    }
}
