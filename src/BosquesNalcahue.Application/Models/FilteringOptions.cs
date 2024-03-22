using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosquesNalcahue.Application.Models
{
    public class FilteringOptions
    {
        public string? ReportType { get; init; }
        public string? OperatorName { get; init; }
        public DateTimeOffset? StartDate { get; init; }
        public DateTimeOffset? EndDate { get; init; }
        public string? ProductType { get; init; }
        public IEnumerable<string>? Species { get; init; }
    }
}
