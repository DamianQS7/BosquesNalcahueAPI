namespace BosquesNalcahue.Contracts.Responses
{
    public class PagedResponse<TResponse>
    {
        public IEnumerable<TResponse> Items { get; set; } = [];
        public required int Page { get; init; }
        public required int PageSize { get; init; }
        public required int TotalCount { get; init; }
        public bool HasNextPage => Page * PageSize < TotalCount;
    }

}
