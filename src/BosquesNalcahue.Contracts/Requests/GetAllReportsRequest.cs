using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosquesNalcahue.Contracts.Requests
{
    public class GetAllReportsRequest
    {
        // Filter properties
        public string? Folio { get; init; }
        public string? OperatorName { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public string? ProductType { get; init; }
        public string? ProductName { get; init; }
        public IEnumerable<string>? Species { get; init; }

        // Sorting properties
        public string? SortBy { get; init; }

        // Pagination properties
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
