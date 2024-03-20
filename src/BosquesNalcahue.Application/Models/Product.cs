namespace BosquesNalcahue.Application.Models
{
    public class Product
    {
        public required string Origin { get; set; }
        public required string Species { get; set; }
        public required double Length { get; set; }
        public required int QuantitySum { get; set; }
        public required double VolumeSum { get; set; }
        public List<ProductMeasurement>? Measurements { get; set; }
    }
}
