namespace BosquesNalcahue.Contracts.Requests;

public class GetAnalyticsByPeriodRequest
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
