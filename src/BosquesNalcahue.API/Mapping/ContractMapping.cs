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
                Folio = request.Folio,
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

        public static GetAnalyticsByPeriodOptions ToOptions(this GetAnalyticsByPeriodRequest request)
        {
            return new GetAnalyticsByPeriodOptions
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
        }

        public static ReportsCountByPeriodResponse ToResponse(this IEnumerable<ReportsCountDocument> reports)
        {
            var response = new ReportsCountByPeriodResponse();

            foreach (var document in reports)
            {
                switch (document.ProductType?.ToLower())
                {
                    case "leña":
                        response.Lena = document.Count;
                        break;
                    case "metro ruma":
                        response.MetroRuma = document.Count;
                        break;
                    case "trozo aserrable":
                        response.TrozoAserrable = document.Count;
                        break;
                    default:
                        throw new Exception($"There is a document with an unknown ProductType: {document.ProductType}");
                }
            }

            return response;
        }

        public static ReportsMonthlyCountResponse ToMonthlyCountResponse(this IEnumerable<ReportsCountDocument> reports)
        {
            var monthlyResponse = new ReportsMonthlyCountResponse();

            foreach (var report in reports)
            {
                switch (report.ProductType?.ToLower())
                {
                    case "leña":
                        monthlyResponse.Lena[report.Month - 1] = report.Count; // Months are 1-indexed, arrays are 0-indexed
                        break;
                    case "metro ruma":
                        monthlyResponse.MetroRuma[report.Month - 1] = report.Count;
                        break;
                    case "trozo aserrable":
                        monthlyResponse.TrozoAserrable[report.Month - 1] = report.Count;
                        break;
                    default:
                        throw new Exception($"Invalid product type: {report.ProductType}");
                }
            }

            return monthlyResponse;
        }

        public static WebPortalUser MapToAppUser(this RegisterUserRequest request)
        {
            return new WebPortalUser
            {
                UserName = request.Email,
                Email = request.Email
            };
        }
    }
}
