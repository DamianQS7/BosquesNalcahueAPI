using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BosquesNalcahue.Application.Entities
{
    [BsonKnownTypes(typeof(SingleProductReport), typeof(MultiProductReport))]
    public class BaseReport
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("_t")] public string? ReportType { get; set; }
        public string? ProductType { get; set; }
        public IEnumerable<string>? Species { get; set; }
        public string? OperatorName { get; set; }
        public string? Folio { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? ClientName { get; set; }
        public string? ClientId { get; set; }
        public string? TruckCompany { get; set; }
        public string? TruckDriver { get; set; }
        public string? TruckDriverId { get; set; }
        public string? TruckPlate { get; set; }
    }
}
