using BosquesNalcahue.API.Controllers;
using BosquesNalcahue.Application.Repositories;
using BosquesNalcahue.Application.Services;
using Microsoft.Extensions.Logging;

namespace BosquesNalcahue.Tests.Unit.Controllers;

public class ReportsControllerTests
{
    private readonly ReportsController _sut;
    private readonly IReportsRepository reportsRepository = Substitute.For<IReportsRepository>();
    private readonly IBlobStorageService blobStorageService = Substitute.For<IBlobStorageService>();
    private readonly IPdfGeneratorService pdfGeneratorService = Substitute.For<IPdfGeneratorService>();
    private readonly ILogger<ReportsController> logger = Substitute.For<ILogger<ReportsController>>();

    public ReportsControllerTests()
    {
        _sut = new(reportsRepository, blobStorageService, pdfGeneratorService, logger);
    }
}
