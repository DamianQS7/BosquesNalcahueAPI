using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using BosquesNalcahue.Contracts.Requests;
using BosquesNalcahue.Contracts.Responses;

namespace BosquesNalcahue.API.Mapping
{
    public static class ContractMapping
    {
        public static GetAllReportsOptions MapToGetAllReportsOptions(this GetAllReportsRequest request)
        {
            return new GetAllReportsOptions
            {
                OperatorName = request.OperatorName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ProductType = request.ProductType,
                ProductName = request.ProductName,
                Species = request.Species,
                SortBy = request.SortBy?.TrimStart('+', '-'),
                SortOrder = request.SortBy is null ? SortOrder.Descending : 
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public static ReportsResponse MapToReportsResponse(this IEnumerable<BaseReport> reports,
        int page, int pageSize, int totalCount)
        {
            return new ReportsResponse
            {
                Items = reports,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
