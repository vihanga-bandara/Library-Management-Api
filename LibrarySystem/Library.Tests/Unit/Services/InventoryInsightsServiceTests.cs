using FluentAssertions;
using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;
using Library.Backend.Application.Services;
using Moq;

namespace Library.Tests.Unit.Services;

public class InventoryInsightsServiceTests
{
    private readonly Mock<ILibraryAnalyticsRepository> _mockRepo;
    private readonly InventoryInsightsService _service;

    public InventoryInsightsServiceTests()
    {
        _mockRepo = new Mock<ILibraryAnalyticsRepository>();
        _service = new InventoryInsightsService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetMostBorrowedBooksAsync_ShouldReturnRepositoryResult()
    {
        // Arrange
        var limit = 10;
        var expectedResult = new List<BorrowedBooksDto>
        {
            new(Guid.NewGuid(), "Book 1", 5),
            new(Guid.NewGuid(), "Book 2", 3)
        };

        _mockRepo.Setup(r => r.GetMostBorrowedBooksAsync(limit))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetMostBorrowedBooksAsync(limit);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _mockRepo.Verify(r => r.GetMostBorrowedBooksAsync(limit), Times.Once);
    }
}
