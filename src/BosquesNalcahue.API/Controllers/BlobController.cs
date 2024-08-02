using BosquesNalcahue.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BosquesNalcahue.API.Controllers
{
    [ApiController]
    [Authorize]
    public class BlobController(IBlobStorageService blobStorageService) : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService = blobStorageService;

        [HttpDelete(Endpoints.Blob.Delete)]
        public async Task<IActionResult> DeleteAsync(Guid blobId)
        {
            var result = await _blobStorageService.DeleteBlobAsync(blobId);

            if (result)
                return Ok();
            
            return BadRequest();
        }

        [HttpGet(Endpoints.Blob.GetUri)]
        public async Task<IActionResult> GetUriAsync(Guid blobId)
        {
            var sasUri = await _blobStorageService.GetUriToBlobAsync(blobId);

            return Ok(new { sasUri });
        }

        [HttpPost(Endpoints.Blob.Upload)]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();

            var fileName = await _blobStorageService.UploadBlobAsync(stream, file.ContentType);

            return Ok(new { fileName });
        }
    }
}
