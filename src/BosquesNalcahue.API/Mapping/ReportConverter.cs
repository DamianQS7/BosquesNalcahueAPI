using BosquesNalcahue.Application.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BosquesNalcahue.API.Mapping
{
    public class ReportConverter : JsonConverter<BaseReport>
    {
        public override BaseReport ReadJson(JsonReader reader, Type objectType, BaseReport existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            switch (jsonObject["ReportType"].Value<string>())
            {
                case "SingleProductReport":
                    var singleProductReport = new SingleProductReport();
                    serializer.Populate(jsonObject.CreateReader(), singleProductReport);
                    return singleProductReport;

                case "MultiProductReport":
                    MultiProductReport multiProductReport = new MultiProductReport();
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

        //public override void WriteJson(JsonWriter writer, BaseReport value, JsonSerializer serializer)
        //{
        //    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        //    if (value is SingleProductReport singleProductReport)
        //    {
        //        serializer.Serialize(writer, singleProductReport);
        //    }
        //    else if (value is MultiProductReport multiProductReport)
        //    {
        //        serializer.Serialize(writer, multiProductReport);
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("Invalid ReportType");
        //    }
        //}

    }
}
