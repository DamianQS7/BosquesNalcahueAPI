using BosquesNalcahue.API.Auth;
using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Repositories;
using BosquesNalcahue.Application.Services;
using BosquesNalcahue.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using QuestPDF.Fluent;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    public class ReportsController(IReportsRepository reportsRepository, IBlobStorageService blobStorageService, 
        IPdfGeneratorService pdfService, ILogger<ReportsController> logger) : ControllerBase
    {
        private readonly IReportsRepository _reportsRepository = reportsRepository;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IPdfGeneratorService _pdfService = pdfService;
        private readonly ILogger<ReportsController> _logger = logger;
        private readonly string _containerName = "reports";

        [Authorize]
        [HttpPost(Endpoints.Reports.Create)]
        public async Task<IActionResult> CreateReport([FromBody] BaseReport report, CancellationToken token = default)
        {
            await _reportsRepository.CreateAsync(report, token);
            _logger.LogInformation("CreateReport: Added a new report to the database with id {reportId}", report.Id);
            return CreatedAtAction(nameof(GetReportById), new {id = report.Id}, report);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete(Endpoints.Reports.Delete)]
        public async Task<IActionResult> DeleteReport([FromRoute] ObjectId id, CancellationToken token = default)
        {
            bool isDeleted = await _reportsRepository.DeleteByIdAsync(id, token);

            if (!isDeleted)
            {
                _logger.LogInformation("DeleteReport: Report with id {reportId} was not found.", id);
                return NotFound();
            }

            _logger.LogInformation("DeleteReport: Deleted report with id {reportId}", id);
            return Ok();
        }

        [Authorize]
        [HttpGet(Endpoints.Reports.GetAll)]
        public async Task<IActionResult> GetAllReports(
            [FromQuery] GetAllReportsRequest request, CancellationToken token = default)
        {
            var filteringOptions = request.MapToGetAllReportsOptions();

            var reports = await _reportsRepository.GetAllAsync(filteringOptions, token);

            int count = await _reportsRepository.GetTotalReports(filteringOptions, token);

            var response = reports.MapToReportsResponse(filteringOptions.Page, filteringOptions.PageSize, count);

            _logger.LogInformation("GetAllReports: Page: {currentPage}, Page Size: {pageSize}.", filteringOptions.Page, filteringOptions.PageSize);

            return Ok(response);
        }

        [Authorize]
        [HttpGet(Endpoints.Reports.GetById)]
        public async Task<IActionResult> GetReportById([FromRoute] ObjectId id, CancellationToken token = default)
        {
            var report = await _reportsRepository.GetByIdAsync(id, token);

            if (report is null)
            {
                _logger.LogInformation("GetReportById: No report was found with id {reportId}", id);
                return NotFound();
            }

            _logger.LogInformation("GetReportById: Found the report with id {reportId}", id);
            return Ok(report);
        }

        [Authorize("Admin")]
        [HttpPut(Endpoints.Reports.Replace)]
        public async Task<IActionResult> ReplaceReportById([FromRoute] ObjectId id,
            [FromBody] BaseReport report, CancellationToken token = default)
        {
            // Create a new report with the same id as the one in the route
            report.Id = id;

            bool isReplaced = await _reportsRepository.ReplaceAsync(report, token);

            if (!isReplaced)
            {
                _logger.LogInformation("ReplaceReportById: No report was found with id {reportId}", id);
                return NotFound();
            }
            _logger.LogInformation("ReplaceReportById: Report with Folio {} updated successfully", report.Folio);

            // Create a new PDF document as byte array
            var pdf = GeneratePdfBasedOnReportProductType(report);
            var stream = new MemoryStream(pdf);
            
            try
            {
                await _blobStorageService.UploadBlobAsync(_containerName, report.FileId!, stream, cancellationToken: token);
                _logger.LogInformation("ReplaceReportById: Updated PDF and report with id {reportId}", id);
                return Ok(report);
            }
            catch (Exception e)
            {
                _logger.LogError("ReplaceReportById: Operation failed with error: {e}", e);
            }
            finally
            {
                stream.Dispose();
            }

            return BadRequest();
        }

        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [HttpPost(Endpoints.Reports.UploadReport)]
        public async Task<IActionResult> UploadReportAsync([FromBody] BaseReport report, CancellationToken token = default)
        {
            // Add the report to the database
            await _reportsRepository.CreateAsync(report, token);
            _logger.LogInformation("UploadReportAsync: Report successfully posted to the database. Starting PDF Generation...");

            // Create a new PDF document as byte array
            var pdf = GeneratePdfBasedOnReportProductType(report);
            var stream = new MemoryStream(pdf);

            try
            {
                // Upload the PDF to the Blob Storage
                string fileName = report.FileId ?? "";
                await _blobStorageService.UploadBlobAsync(_containerName, fileName, stream, cancellationToken: token);

                _logger.LogInformation("UploadReportAsync: PDF successfully generated and uploaded to Blob Storage");

                // Return the file to Download
                return File(pdf, "application/pdf", fileName + ".pdf");
            }
            catch (Exception)
            {
                // Manually delete the report from the database
                await _reportsRepository.DeleteByIdAsync(report.Id, token);
                stream.Dispose();
                _logger.LogError("UploadReportAsync: Operation failed; deleting report entry from the DB.");
            }

            return BadRequest();
        }

        private byte[] GeneratePdfBasedOnReportProductType(BaseReport report) 
        {
            // Create a new PDF document as byte array
            var document = report.ProductType switch
            {
                "Leña" => _pdfService.CreateLenaReport((SingleProductReport)report),
                "Metro Ruma" => _pdfService.CreateMetroRumaReport((SingleProductReport)report),
                "Trozo Aserrable" => _pdfService.CreateTrozoAserrableReport((MultiProductReport)report),
                _ => throw new ArgumentException("Invalid product type")
            };

            return document.GeneratePdf();
        }
    }
}
