using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        [HttpGet(Endpoints.Reports.GetAll)]
        public async Task<IActionResult> GetAllReports(CancellationToken token = default)
        {
            //var reports = await _reportsRepository.GetAllAsync(token);
            //return Ok(reports);
            var reports = await _reportsRepository.GetAllAsync(token);
            return Ok(reports);
        }

        [HttpPost(Endpoints.Reports.Create)]
        public async Task<IActionResult> CreateReport([FromBody] BaseReport report, CancellationToken token = default)
        {
            await _reportsRepository.CreateAsync(report, token);
            return Ok(report);
        }

        
    }
}
