namespace BosquesNalcahue.Application.Models;

public class MongoDbOptions : IMongoDbOptions
{
    public const string OptionsName = nameof(MongoDbOptions);
    public string Server { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Collection { get; set; } = null!;
}

public interface IMongoDbOptions 
{
    public string Server { get; set; }
    public string Database { get; set; }
    public string Collection { get; set; }
}