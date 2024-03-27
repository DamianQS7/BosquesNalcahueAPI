using Newtonsoft.Json;

namespace BosquesNalcahue.Tests.Unit.Converters
{
    public class ReportConverterTests
    {
        [Theory]
        [InlineData("{\"ReportType\":\"SingleProductReport\"}", typeof(SingleProductReport))]
        [InlineData("{\"ReportType\":\"MultiProductReport\"}", typeof(MultiProductReport))]
        public void ReadJson_ShouldCorrectlyDeserializeBasedOnReportType(string json, Type expectedType)
        {
            // Arrange
            var converter = new ReportConverter();
            var reader = new JsonTextReader(new StringReader(json));
            var serializer = new JsonSerializer();

            // Act
            var result = converter.ReadJson(reader, typeof(BaseReport), null, serializer);

            // Assert
            result.Should().BeOfType(expectedType);
        }

        [Fact]
        public void ReadJson_ShouldThrowInvalidOperationException_WhenReportTypeIsInvalid()
        {
            // Arrange
            var converter = new ReportConverter();
            var reader = new JsonTextReader(new StringReader("{\"ReportType\":\"InvalidReportType\"}"));
            var serializer = new JsonSerializer();

            // Act
            Action act = () => converter.ReadJson(reader, typeof(BaseReport), null, serializer);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Invalid ReportType");
        }
    }
}
