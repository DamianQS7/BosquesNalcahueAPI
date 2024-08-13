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
    public class ReportsController(IReportsRepository reportsRepository, IBlobStorageService blobStorageService, PdfGeneratorService pdfService) : ControllerBase
    {
        private readonly IReportsRepository _reportsRepository = reportsRepository;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly PdfGeneratorService _pdfService = pdfService;

        [HttpPost(Endpoints.Reports.Create)]
        public async Task<IActionResult> CreateReport([FromBody] BaseReport report, CancellationToken token = default)
        {
            await _reportsRepository.CreateAsync(report, token);

            return CreatedAtAction(nameof(GetReportById), new {id = report.Id}, report);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete(Endpoints.Reports.Delete)]
        public async Task<IActionResult> DeleteReport([FromRoute] ObjectId id, CancellationToken token = default)
        {
            bool isDeleted = await _reportsRepository.DeleteByIdAsync(id, token);

            if (!isDeleted)
                return NotFound();

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
            
            return Ok(response);
        }

        [Authorize]
        [HttpGet(Endpoints.Reports.GetById)]
        public async Task<IActionResult> GetReportById([FromRoute] ObjectId id, CancellationToken token = default)
        {
            var report = await _reportsRepository.GetByIdAsync(id, token);

            if (report is null)
                return NotFound();

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
                return NotFound();

            return Ok(report);
        }

        [HttpPost(Endpoints.Reports.UploadSingleProductReport)]
        public async Task<IActionResult> UploadSingleProductReportAsync([FromBody] SingleProductReport report, CancellationToken token = default)
        {
            // Add the report to the database
            await _reportsRepository.CreateAsync(report, token);
    
            try
            {
                // Create a new PDF document as byte array
                var document = report.ProductType switch
                {
                    "Lena" => _pdfService.CreateLenaReport(report),
                    "Metro Ruma" => _pdfService.CreateMetroRumaReport(report),
                    _ => throw new ArgumentException("Invalid product type")
                };

                byte[] pdf = document.GeneratePdf();

                // Upload the PDF to the Blob Storage
                using Stream stream = new MemoryStream(pdf);
                var blobId = await _blobStorageService.UploadBlobAsync(stream, cancellationToken: token);

                // Return the file to Download
                return File(pdf, "application/pdf", blobId.ToString());
            }
            catch (Exception)
            {
                // Manually delete the report from the database
                await _reportsRepository.DeleteByIdAsync(report.Id, token);
            }

            return BadRequest();
        }

        [HttpPost(Endpoints.Reports.UploadMultiProductReport)]
        public async Task<IActionResult> UploadMultiProductReportAsync([FromBody] MultiProductReport report, CancellationToken token = default)
        {
            // Add the report to the database
            await _reportsRepository.CreateAsync(report, token);

            try
            {
                // Create a new PDF document as byte array
                var document = _pdfService.CreateTrozoAserrableReport(report);

                byte[] pdf = document.GeneratePdf();

                // Upload the PDF to the Blob Storage
                using Stream stream = new MemoryStream(pdf);
                var blobId = await _blobStorageService.UploadBlobAsync(stream, cancellationToken: token);

                // Return the file to Download
                return File(pdf, "application/pdf", blobId.ToString());
            }
            catch (Exception)
            {
                // Manually delete the report from the database
                await _reportsRepository.DeleteByIdAsync(report.Id, token);
            }

            return BadRequest();
        }
    }
}
