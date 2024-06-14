using BosquesNalcahue.Application.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BosquesNalcahue.API.Converters
{
    public class ReportConverter : JsonConverter<BaseReport>
    {
        public override BaseReport ReadJson(JsonReader reader, Type objectType, BaseReport existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var reportType = jsonObject["ReportType"]?.Value<string>() ?? jsonObject["reportType"]?.Value<string>();

            switch (reportType)
            {
                case "SingleProductReport":
                    SingleProductReport singleProductReport = new();
                    serializer.Populate(jsonObject.CreateReader(), singleProductReport);
                    return singleProductReport;

                case "MultiProductReport":
                    MultiProductReport multiProductReport = new();
                    serializer.Populate(jsonObject.CreateReader(), multiProductReport);
                    return multiProductReport;

                default:
                    throw new InvalidOperationException("Invalid ReportType");
            }
        }

        public override void WriteJson(JsonWriter writer, BaseReport? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
