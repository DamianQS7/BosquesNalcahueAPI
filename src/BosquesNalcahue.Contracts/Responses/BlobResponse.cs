using BosquesNalcahue.Application.Models;

namespace BosquesNalcahue.Contracts.Responses;

public class BlobResponse
{
    public BlobDto Blob { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
}
