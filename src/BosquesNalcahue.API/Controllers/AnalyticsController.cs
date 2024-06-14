using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application.Repositories;
using BosquesNalcahue.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    public class AnalyticsController(IAnalyticsRepository analyticsRepository) : Controller
    {
        private readonly IAnalyticsRepository _analyticsRepository = analyticsRepository;

        [HttpGet(Endpoints.Analytics.MonthlyCountBreakdown)]
        public async Task<IActionResult> GetMonthlyCountBreakdown(CancellationToken token = default)
        {
            var countDocuments = await _analyticsRepository.GetMonthlyCountBreakdownAsync(token);

            var count = countDocuments.ToMonthlyCountResponse();

            return Ok(count);
        }

        [HttpGet(Endpoints.Analytics.ReportsCountByMonth)]
        public async Task<IActionResult> GetReportsCountByMonth([FromRoute] int month, CancellationToken token = default)
        {
            var countDocument = await _analyticsRepository.GetReportCountByMonthAsync(month, token);

            var count = countDocument.ToResponse();

            return Ok(count);
        }

        [HttpGet(Endpoints.Analytics.ReportsCountByPeriod)]
        public async Task<IActionResult> GetReportCountByPeriod([FromQuery] GetAnalyticsByPeriodRequest request, CancellationToken token = default)
        {
            var options = request.ToOptions();

            var countDocument = await _analyticsRepository.GetReportCountByPeriodAsync(options, token);

            var count = countDocument.ToResponse();

            return Ok(count);
        }
    }
}
