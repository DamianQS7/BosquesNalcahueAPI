using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application.Repositories;
using BosquesNalcahue.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BosquesNalcahue.API.Controllers
{
    //[Authorize]
    [ApiController]
    public class AnalyticsController(IAnalyticsRepository analyticsRepository, ILogger<AnalyticsController> logger) : ControllerBase
    {
        private readonly IAnalyticsRepository _analyticsRepository = analyticsRepository;
        private readonly ILogger<AnalyticsController> _logger = logger;

        [HttpGet(Endpoints.Analytics.MonthlyCountBreakdown)]
        public async Task<IActionResult> GetMonthlyCountBreakdown(CancellationToken token = default)
        {
            _logger.LogInformation("GetMonthlyCountBreakdown: Request incoming.");

            var countDocuments = await _analyticsRepository.GetMonthlyCountBreakdownAsync(token);

            var count = countDocuments.ToMonthlyCountResponse();

            return Ok(count);
        }

        [HttpGet(Endpoints.Analytics.ReportsCountByMonth)]
        public async Task<IActionResult> GetReportsCountByMonth([FromRoute] int month, CancellationToken token = default)
        {
            _logger.LogInformation("GetReportsCountByMonth: Request incoming.");

            var countDocument = await _analyticsRepository.GetReportCountByMonthAsync(month, token);

            var count = countDocument.ToResponse();

            return Ok(count);
        }

        [HttpGet(Endpoints.Analytics.ReportsCountByPeriod)]
        public async Task<IActionResult> GetReportCountByPeriod([FromQuery] GetAnalyticsByPeriodRequest request, CancellationToken token = default)
        {
            _logger.LogInformation("GetReportCountByPeriod: Request incoming.");

            var options = request.ToOptions();

            var countDocument = await _analyticsRepository.GetReportCountByPeriodAsync(options, token);

            var count = countDocument.ToResponse();

            return Ok(count);
        }
    }
}
