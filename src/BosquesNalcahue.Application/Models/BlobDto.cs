namespace BosquesNalcahue.Application.Models;

public record BlobDto(string? Name, string? Url, string? ContentType, Stream? Content);
