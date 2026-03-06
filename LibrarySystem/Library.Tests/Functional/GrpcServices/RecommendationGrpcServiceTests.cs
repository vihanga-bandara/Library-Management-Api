using FluentAssertions;
using Libary.Backend.Grpc.Services;
using Library.Backend.Application.Models;
using Library.Backend.Application.Services;
using Library.Shared.Contracts.Recommendation.V1;
using Library.Tests.Functional.GrpcServices;
using Moq;

namespace Library.Tests.Functional.GrpcServices;

public class RecommendationGrpcServiceTests
{
    private readonly Mock<IRecommendationService> _mockService;
    private readonly RecommendationGrpcService _grpcService;

    public RecommendationGrpcServiceTests()
    {
        _mockService = new Mock<IRecommendationService>();
        _grpcService = new RecommendationGrpcService(_mockService.Object);
    }

    [Fact]
    public async Task GetOtherBorrowedBooks_ShouldMapDtosToProtoCorrectly()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var recommendedBookId = Guid.NewGuid();
        var request = new GetOtherBorrowedBooksRequest { BookId = bookId.ToString() };
        var recommendations = new List<BorrowedBooksDto>
        {
            new(recommendedBookId, "Recommended Book", 5)
        };

        _mockService.Setup(s => s.GetOtherBorrowedBooksAsync(bookId, 10))
            .ReturnsAsync(recommendations);

        // Act
        var response = await _grpcService.GetOtherBorrowedBooks(request, TestServerCallContext.Create());

        // Assert
        response.RecommendedBooks.Should().HaveCount(1);
        response.RecommendedBooks[0].BookId.Should().Be(recommendedBookId.ToString());
        response.RecommendedBooks[0].Title.Should().Be("Recommended Book");
    }
}
