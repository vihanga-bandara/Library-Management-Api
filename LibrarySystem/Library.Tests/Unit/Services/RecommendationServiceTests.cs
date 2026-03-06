using FluentAssertions;
using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;
using Library.Backend.Application.Services;
using Moq;

namespace Library.Tests.Unit.Services;

public class RecommendationServiceTests
{
    private readonly Mock<ILibraryAnalyticsRepository> _mockRepo;
    private readonly RecommendationService _service;

    public RecommendationServiceTests()
    {
        _mockRepo = new Mock<ILibraryAnalyticsRepository>();
        _service = new RecommendationService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetOtherBorrowedBooksAsync_ShouldReturnRepositoryResult()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var limit = 10;
        var expectedResult = new List<BorrowedBooksDto>
        {
            new(Guid.NewGuid(), "Recommended Book 1", 5),
            new(Guid.NewGuid(), "Recommended Book 2", 3)
        };

        _mockRepo.Setup(r => r.GetOtherBorrowedBooksAsync(bookId, limit))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetOtherBorrowedBooksAsync(bookId, limit);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _mockRepo.Verify(r => r.GetOtherBorrowedBooksAsync(bookId, limit), Times.Once);
    }
}
