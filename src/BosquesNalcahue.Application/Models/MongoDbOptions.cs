namespace BosquesNalcahue.Application.Models;

public class MongoDbOptions : IMongoDbOptions
{
    public const string OptionsName = nameof(MongoDbOptions);
    public string Server { get; set; }
    public string Database { get; set; }
    public string Collection { get; set; }
}

public interface IMongoDbOptions {}