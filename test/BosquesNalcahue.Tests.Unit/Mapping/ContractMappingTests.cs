namespace BosquesNalcahue.Tests.Unit.Mapping
{
    public class ContractMappingTests
    {
        [Fact]
        public void MapToGetAllReportsOptions_ShouldCorrectlyMapToOptions_WhenAllPropertiesAreGiven()
        {
            // Arrange
            var request = new GetAllReportsRequest
            {
                OperatorName = "Test Operator",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now,
                ProductType = "Test Product Type",
                ProductName = "Test Product Name",
                Species = ["Test Species 1", "Test Species 2"],
                SortBy = "+TestSort",
                Page = 2,
                PageSize = 20
            };

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.OperatorName.Should().Be(request.OperatorName);
            result.StartDate.Should().Be(request.StartDate);
            result.EndDate.Should().Be(request.EndDate);
            result.ProductType.Should().Be(request.ProductType);
            result.ProductName.Should().Be(request.ProductName);
            result.Species.Should().BeEquivalentTo(request.Species);
            result.SortBy.Should().Be(request.SortBy.TrimStart('+', '-'));
            result.SortOrder.Should().Be(SortOrder.Ascending);
        }

        [Fact]
        public void MapToGetAllReportsOptions_ShouldCorrectlyMapSortOrder_WhenSortByIsNull()
        {
            // Arrange
            var request = new GetAllReportsRequest();

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.SortOrder.Should().Be(SortOrder.Descending);
        }

        [Fact]
        public void MapToGetAllReportsOptions_ShouldCorrectlyMapSortOrder_WhenSortByIsNegative()
        {
            // Arrange
            var request = new GetAllReportsRequest { SortBy = "-Date"};

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.SortOrder.Should().Be(SortOrder.Descending);
        }

        [Fact]
        public void MapToGetAllReportsOptions_ShouldCorrectlyMapSortOrder_WhenSortByIsPositive()
        {
            // Arrange
            var request = new GetAllReportsRequest { SortBy = "+Date" };

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.SortOrder.Should().Be(SortOrder.Ascending);
        }

        [Fact]
        public void MapToGetAllReportsOptions_ShouldCorrectlyMapPagination_WhenNoDataIsProvided()
        {
            // Arrange
            var request = new GetAllReportsRequest();

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public void MapToReportsResponse_ShouldCorrectlyMapFromReportsAndPaginationInfo()
        {
            // Arrange
            var reports = new List<BaseReport>
            {
                new SingleProductReport(),
                new MultiProductReport()
            };
            int page = 1;
            int pageSize = 10;
            int totalCount = 20;

            // Act
            var result = reports.MapToReportsResponse(page, pageSize, totalCount);

            // Assert
            result.Items.Should().BeEquivalentTo(reports);
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(totalCount);
            result.HasNextPage.Should().BeTrue();
        }

        [Fact]
        public void ToOptions_ShouldCorrectlyMap_FromGetAnalyticsByPeriodRequest()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            var request = new GetAnalyticsByPeriodRequest { StartDate = startDate, EndDate = endDate };

            // Act
            var options = request.ToOptions();

            // Assert
            options.Should().NotBeNull();
            options.StartDate.Should().Be(startDate);
            options.EndDate.Should().Be(endDate);
        }

        [Theory]
        [InlineData("leña", "metro ruma", 10, 15)]
        public void ToResponse_ShouldCorrectlyMap_ToTheCorrespondingMonth(string productType1, string productType2, 
            int month, int count)
        {
            // Arrange

            var reports = new List<ReportsCountDocument>
            {
                new () { ProductType = productType1, Count = count, Month = month },
                new () { ProductType = productType2, Count = count, Month = month }
            };

            // Act
            var response = reports.ToMonthlyCountResponse();

            // Assert
            response.Should().NotBeNull();
            response.Lena[month - 1].Should().Be(count);
            response.MetroRuma[month - 1].Should().Be(count);
            response.TrozoAserrable[month].Should().Be(0);
        }
    }
}
