namespace BosquesNalcahue.Contracts.Responses
{
    public class ValidationErrorResponse
    {
        public required IEnumerable<ValidationResponse> Errors { get; init; }
    }

    public record ValidationResponse(string Property, string Message);
}
