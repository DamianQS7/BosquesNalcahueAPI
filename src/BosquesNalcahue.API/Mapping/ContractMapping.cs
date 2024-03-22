using BosquesNalcahue.Application.Models;
using BosquesNalcahue.Contracts.Requests;

namespace BosquesNalcahue.API.Mapping
{
    public static class ContractMapping
    {
        public static FilteringOptions MapToFilteringOptions(this GetAllReportsRequest request)
        {
            return new FilteringOptions
            {
                ReportType = request.ReportType,
                OperatorName = request.OperatorName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ProductType = request.ProductType,
                Species = request.Species
            };
        }
    }
}
