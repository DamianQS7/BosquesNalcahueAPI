namespace BosquesNalcahue.Tests.Unit.Mapping
{
    public class ContractMappingTests
    {
        [Fact]
        public void MapToFilteringOptions_ShouldCorrectlyMapToOptions_WhenAllPropertiesAreGiven()
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
        public void MapToFilteringOptions_ShouldCorrectlyMapSortOrder_WhenSortByIsNull()
        {
            // Arrange
            var request = new GetAllReportsRequest();

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.SortOrder.Should().Be(SortOrder.Descending);
        }

        [Fact]
        public void MapToFilteringOptions_ShouldCorrectlyMapSortOrder_WhenSortByIsNegative()
        {
            // Arrange
            var request = new GetAllReportsRequest { SortBy = "-Date"};

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.SortOrder.Should().Be(SortOrder.Descending);
        }

        [Fact]
        public void MapToFilteringOptions_ShouldCorrectlyMapSortOrder_WhenSortByIsPositive()
        {
            // Arrange
            var request = new GetAllReportsRequest { SortBy = "+Date" };

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.SortOrder.Should().Be(SortOrder.Ascending);
        }

        [Fact]
        public void MapToFilteringOptions_ShouldCorrectlyMapPagination_WhenNoDataIsProvided()
        {
            // Arrange
            var request = new GetAllReportsRequest();

            // Act
            var result = request.MapToGetAllReportsOptions();

            // Assert
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(15);
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
    }
}
