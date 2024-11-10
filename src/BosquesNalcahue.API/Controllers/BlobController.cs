using BosquesNalcahue.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    //[Authorize]
    public class BlobController(IBlobStorageService blobStorageService, ILogger<BlobController> logger) : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly ILogger<BlobController> _logger = logger;

        [HttpDelete(Endpoints.Blob.Delete)]
        public async Task<IActionResult> DeleteAsync(string containerName, string blobId)
        {
            var result = await _blobStorageService.DeleteBlobAsync(containerName, blobId);

            if (result)
            {
                _logger.LogInformation("DeleteAsync: Blob with id {blobId} was deleted.", blobId);
                return Ok();
            }

            _logger.LogInformation("DeleteAsync: No blob was found with id {blobId}", blobId);
            return BadRequest();
        }

        [HttpGet(Endpoints.Blob.GetUri)]
        public async Task<IActionResult> GetUriAsync(string containerName, string blobId)
        {
            _logger.LogInformation("GetUriAsync: Request incoming for blob with id {blobId}", blobId);

            var sasUri = await _blobStorageService.GetSasUriToBlobAsync(containerName, blobId);
            
            return Ok(new { sasUri });
        }

        [HttpPost(Endpoints.Blob.Upload)]
        public async Task<IActionResult> UploadAsync([FromRoute] string containerName, IFormFile file, CancellationToken token)
        {
            _logger.LogInformation("UploadAsync: Request incoming.");

            using Stream stream = file.OpenReadStream();

            await _blobStorageService.UploadBlobAsync(containerName, file.FileName, stream, file.ContentType, cancellationToken: token);

            return Ok(new { file.FileName });
        }
    }
}
