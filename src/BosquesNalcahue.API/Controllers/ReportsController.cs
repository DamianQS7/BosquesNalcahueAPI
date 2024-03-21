using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsController(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        [HttpPost(Endpoints.Reports.Create)]
        public async Task<IActionResult> CreateReport([FromBody] BaseReport report, CancellationToken token = default)
        {
            await _reportsRepository.CreateAsync(report, token);
            return Ok(report);
        }

        [HttpDelete(Endpoints.Reports.Delete)]
        public async Task<IActionResult> DeleteReport([FromRoute] ObjectId id, CancellationToken token = default)
        {
            bool isDeleted = await _reportsRepository.DeleteByIdAsync(id, token);

            if (!isDeleted)
                return NotFound();

            return Ok();
        }

        [HttpGet(Endpoints.Reports.GetAll)]
        public async Task<IActionResult> GetAllReports(CancellationToken token = default)
        {
            var reports = await _reportsRepository.GetAllAsync(token);
            return Ok(reports);
        }

        [HttpGet(Endpoints.Reports.GetById)]
        public async Task<IActionResult> GetReportById([FromRoute] ObjectId id, CancellationToken token = default)
        {
            var report = await _reportsRepository.GetByIdAsync(id, token);

            if (report is null)
                return NotFound();

            return Ok(report);
        }

        [HttpPut(Endpoints.Reports.Replace)]
        public async Task<IActionResult> ReplaceReportById([FromRoute] ObjectId id,
            [FromBody] BaseReport report, CancellationToken token = default)
        {
            // Create a new report with the same id as the one in the route
            report.Id = id;

            bool isReplaced = await _reportsRepository.ReplaceAsync(report, token);

            if (!isReplaced)
                return NotFound();

            return Ok(report);
        }


    }
}
