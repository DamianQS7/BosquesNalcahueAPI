namespace BosquesNalcahue.Application.Models
{
    public class MongoIdentityOptions
    {
        public const string OptionsName = nameof(MongoIdentityOptions);
        public string Server { get; set; } = null!;
        public string Database { get; set; } = null!;
    }
}
