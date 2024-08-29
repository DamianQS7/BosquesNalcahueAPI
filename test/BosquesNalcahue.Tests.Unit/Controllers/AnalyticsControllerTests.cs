using BosquesNalcahue.API.Controllers;
using BosquesNalcahue.Application.Repositories;
using BosquesNalcahue.Application.Services;
using BosquesNalcahue.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BosquesNalcahue.Tests.Unit.Controllers;

public class AnalyticsControllerTests
{
    private readonly AnalyticsController _sut;
    private readonly IAnalyticsRepository analyticsRepository = Substitute.For<IAnalyticsRepository>();
    private readonly ILogger<AnalyticsController> logger = Substitute.For<ILogger<AnalyticsController>>();

    public AnalyticsControllerTests()
    {
        _sut = new(analyticsRepository, logger);
    }

    [Fact]
    public async Task GetMonthlyBreakdown_ShouldMapCorrectlyToReportsMonthlyCountResponse_WhenRepositoryProvidesList()
    {
        // Arrange

        var monthlyCounts = GenerateListOfReportsCountDocument();

        ReportsMonthlyCountResponse expected = new()
        {
            MetroRuma = [0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            TrozoAserrable = [0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        };

        analyticsRepository.GetMonthlyCountBreakdownAsync().Returns(monthlyCounts);

        // Act
        var result = await _sut.GetMonthlyCountBreakdown() as OkObjectResult;

        // Assert
        result?.Value.Should().NotBeNull();
        result?.Value.Should().BeEquivalentTo(expected);
        result?.Value.Should().BeOfType<ReportsMonthlyCountResponse>();
    }

    [Fact]
    public async Task GetReportsCountByMonth_ShouldMapCorrectlyToReportsCountByPeriodResponse()
    {
        // Arrange
        var monthlyCounts = GenerateListOfReportsCountDocument();

        ReportsCountByPeriodResponse expected = new() { Lena = 0, MetroRuma = 10, TrozoAserrable = 12 };

        analyticsRepository.GetReportCountByMonthAsync(Arg.Any<int>()).Returns(monthlyCounts);

        // Act
        var result = await _sut.GetReportsCountByMonth(2) as OkObjectResult;

        // Assert
        result?.Value.Should().BeEquivalentTo(expected);
        result?.Value.Should().NotBeNull();
        result?.Value.Should().BeOfType<ReportsCountByPeriodResponse>();
    }

    [Fact]
    public async Task GetReportCountByPeriod_ShouldMapCorrectlyToReportsCountByPeriodResponse()
    {
        // Arrange
        var monthlyCounts = GenerateListOfReportsCountDocument();

        ReportsCountByPeriodResponse expected = new() { Lena = 0, MetroRuma = 10, TrozoAserrable = 12 };

        analyticsRepository.GetReportCountByPeriodAsync(Arg.Any<GetAnalyticsByPeriodOptions>()).Returns(monthlyCounts);

        GetAnalyticsByPeriodRequest request = new() { StartDate = DateTime.Now, EndDate = DateTime.Now };

        // Act
        var result = await _sut.GetReportCountByPeriod(request) as OkObjectResult;

        // Assert
        result?.Value.Should().BeEquivalentTo(expected);
        result?.Value.Should().NotBeNull();
        result?.Value.Should().BeOfType<ReportsCountByPeriodResponse>();
    }

    private List<ReportsCountDocument> GenerateListOfReportsCountDocument() => [
            new() {ProductType = "Metro Ruma", Month = 2, Count = 10},
            new() {ProductType = "Trozo Aserrable", Month = 2, Count = 12}
        ];
}
