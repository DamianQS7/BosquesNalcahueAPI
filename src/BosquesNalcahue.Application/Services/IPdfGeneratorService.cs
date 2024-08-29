using BosquesNalcahue.Application.Entities;
using QuestPDF.Infrastructure;

namespace BosquesNalcahue.Application.Services
{
    public interface IPdfGeneratorService
    {
        IDocument CreateLenaReport(SingleProductReport model);
        IDocument CreateMetroRumaReport(SingleProductReport model);
        IDocument CreateTrozoAserrableReport(MultiProductReport model);
    }
}
