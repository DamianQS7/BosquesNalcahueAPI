namespace BosquesNalcahue.Application.Models
{
    public class FilteringOptions
    {
        // Filters
        public string? OperatorName { get; init; }
        public DateTimeOffset? StartDate { get; init; }
        public DateTimeOffset? EndDate { get; init; }
        public string? ProductType { get; init; }
        public string? ProductName { get; init; }
        public IEnumerable<string>? Species { get; init; }

        // Sorting
        public string? SortBy { get; init; }
        public SortOrder? SortOrder { get; init; }
        
    }

    public enum SortOrder
    {
        Unsorted,
        Ascending,
        Descending
    }
}
