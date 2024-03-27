using BosquesNalcahue.Application.Models;
using BosquesNalcahue.Contracts.Requests;

namespace BosquesNalcahue.API.Mapping
{
    public static class ContractMapping
    {
        public static GetAllReportsOptions MapToFilteringOptions(this GetAllReportsRequest request)
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
                request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending
            };
        }
    }
}
