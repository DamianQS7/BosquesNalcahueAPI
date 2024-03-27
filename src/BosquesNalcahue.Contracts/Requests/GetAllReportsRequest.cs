using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosquesNalcahue.Contracts.Requests
{
    public class GetAllReportsRequest
    {
        public string? OperatorName { get; init; }
        public DateTimeOffset? StartDate { get; init; }
        public DateTimeOffset? EndDate { get; init; }
        public string? ProductType { get; init; }
        public string? ProductName { get; init; }
        public IEnumerable<string>? Species { get; init; }
        public string? SortBy { get; init; }
    }
}
