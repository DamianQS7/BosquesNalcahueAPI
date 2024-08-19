using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.PdfComponents
{
    public class SingleRow(int size, string label, string value) : IComponent
    {
        public int ColumnSize { get; set; } = size;
        public string ColumnLabel { get; set; } = label;
        public string ColumnValue { get; set; } = value;

        public void Compose(IContainer container)
        {
            container.Row(row =>
            {
                row.ConstantItem(ColumnSize)
                   .Border(1)
                   .Text(ColumnLabel);

                row.RelativeItem()
                   .Border(1)
                   .AlignCenter()
                   .Text(ColumnValue);
            });
        }
    }
}
