using BosquesNalcahue.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    public class BlobController(IBlobStorageService blobStorageService) : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService = blobStorageService;

        [HttpPost("api/blob/upload")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();

            var (fileName, fileUrl) = await _blobStorageService.UploadBlobAsync(stream, file.ContentType);

            return Ok(new { fileName, fileUrl });
        }
    }
}
