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

        //public static ReportsMonthlyCountResponse ToMonthlyCountResponse(this IEnumerable<ReportsCountDocument> reports)
        //{
        //    var monthlyResponse = new ReportsMonthlyCountResponse();

        //    var groupedByMonth = reports.GroupBy(r => r.Month);

        //    foreach (var group in groupedByMonth)
        //    {
        //        switch (group.Key)
        //        {
        //            case 1:
        //                monthlyResponse.January = group.ToResponse();
        //                break;
        //            case 2:
        //                monthlyResponse.February = group.ToResponse();
        //                break;
        //            case 3:
        //                monthlyResponse.March = group.ToResponse();
        //                break;
        //            case 4:
        //                monthlyResponse.April = group.ToResponse();
        //                break;
        //            case 5:
        //                monthlyResponse.May = group.ToResponse();
        //                break;
        //            case 6:
        //                monthlyResponse.June = group.ToResponse();
        //                break;
        //            case 7:
        //                monthlyResponse.July = group.ToResponse();
        //                break;
        //            case 8:
        //                monthlyResponse.August = group.ToResponse();
        //                break;
        //            case 9:
        //                monthlyResponse.September = group.ToResponse();
        //                break;
        //            case 10:
        //                monthlyResponse.October = group.ToResponse();
        //                break;
        //            case 11:
        //                monthlyResponse.November = group.ToResponse();
        //                break;
        //            case 12:
        //                monthlyResponse.December = group.ToResponse();
        //                break;
        //            default:
        //                throw new Exception($"Invalid month: {group.Key}");
        //        }

        //    }

        //    return monthlyResponse;
        //}

        public static ReportsMonthlyCountResponse ToMonthlyCountResponse(this IEnumerable<ReportsCountDocument> reports)
        {
            var monthlyResponse = new ReportsMonthlyCountResponse();

            var groupedByMonth = reports.GroupBy(r => r.Month);

            foreach (var group in groupedByMonth)
            {
                switch (group.Key)
                {
                    case 1:
                        monthlyResponse.January = group.ToResponse();
                        break;
                    case 2:
                        monthlyResponse.February = group.ToResponse();
                        break;
                    case 3:
                        monthlyResponse.March = group.ToResponse();
                        break;
                    case 4:
                        monthlyResponse.April = group.ToResponse();
                        break;
                    case 5:
                        monthlyResponse.May = group.ToResponse();
                        break;
                    case 6:
                        monthlyResponse.June = group.ToResponse();
                        break;
                    case 7:
                        monthlyResponse.July = group.ToResponse();
                        break;
                    case 8:
                        monthlyResponse.August = group.ToResponse();
                        break;
                    case 9:
                        monthlyResponse.September = group.ToResponse();
                        break;
                    case 10:
                        monthlyResponse.October = group.ToResponse();
                        break;
                    case 11:
                        monthlyResponse.November = group.ToResponse();
                        break;
                    case 12:
                        monthlyResponse.December = group.ToResponse();
                        break;
                    default:
                        throw new Exception($"Invalid month: {group.Key}");
                }

            }

            return monthlyResponse;
        }
    }
}
