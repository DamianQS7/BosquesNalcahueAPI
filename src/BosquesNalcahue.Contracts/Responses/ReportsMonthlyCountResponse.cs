namespace BosquesNalcahue.Contracts.Responses;

public class ReportsMonthlyCountResponse
{
    public int[] Lena { get; set; } = new int[12];
    public int[] MetroRuma { get; set; } = new int[12];
    public int[] TrozoAserrable { get; set; } = new int[12];
}
