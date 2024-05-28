namespace BosquesNalcahue.Application.Models
{
    public class GetAllReportsOptions
    {
        // Filters
        public string? Folio { get; init; }
        public string? OperatorName { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public string? ProductType { get; init; }
        public string? ProductName { get; init; }
        public IEnumerable<string>? Species { get; init; }

        // Sorting
        public string? SortBy { get; init; }
        public SortOrder? SortOrder { get; init; }

        // Pagination
        public int Page { get; init; }
        public int PageSize { get; init; }
        
    }

    public enum SortOrder
    {
        Unsorted,
        Ascending,
        Descending
    }
}
