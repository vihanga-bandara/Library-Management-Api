using FluentAssertions;
using Library.Backend.Application.Models;
using Library.Backend.Application.Services;
using Library.Backend.Grpc.Services;
using Library.Shared.Contracts.Inventory.V1;
using Library.Tests.Functional.GrpcServices;
using Moq;

namespace Library.Tests.Functional.GrpcServices;

public class InventoryGrpcServiceTests
{
    private readonly Mock<IInventoryInsightsService> _mockService;
    private readonly InventoryGrpcService _grpcService;

    public InventoryGrpcServiceTests()
    {
        _mockService = new Mock<IInventoryInsightsService>();
        _grpcService = new InventoryGrpcService(_mockService.Object);
    }

    [Fact]
    public async Task GetMostBorrowedBooks_ShouldMapDtosToProtoCorrectly()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var request = new GetMostBorrowedBooksRequest { Limit = 5 };
        var books = new List<BorrowedBooksDto>
        {
            new(bookId, "Popular Book", 10)
        };

        _mockService.Setup(s => s.GetMostBorrowedBooksAsync(5))
            .ReturnsAsync(books);

        // Act
        var response = await _grpcService.GetMostBorrowedBooks(request, TestServerCallContext.Create());

        // Assert
        response.BorrowedBooks.Should().HaveCount(1);
        response.BorrowedBooks[0].Id.Should().Be(bookId.ToString());
        response.BorrowedBooks[0].Title.Should().Be("Popular Book");
    }
}
