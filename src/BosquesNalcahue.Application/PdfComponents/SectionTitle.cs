using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class SectionTitle(string title, int size) : IComponent
    {
        public string Title { get; set; } = title;
        public int SubtitleSize { get; set; } = size;

        public void Compose(IContainer container)
        {
            container.Text(Title)
                     .FontSize(SubtitleSize)
                     .Bold();
        }
    }
}
