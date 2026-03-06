using FluentAssertions;
using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;
using Library.Backend.Application.Services;
using Moq;

namespace Library.Tests.Unit.Services;

public class UserActivityServiceTests
{
    private readonly Mock<ILibraryAnalyticsRepository> _mockRepo;
    private readonly UserActivityService _service;

    public UserActivityServiceTests()
    {
        _mockRepo = new Mock<ILibraryAnalyticsRepository>();
        _service = new UserActivityService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetTopBorrowersAsync_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 12, 31);
        var limit = 10;
        var expectedResult = new List<UserBorrowSummaryDto>
        {
            new(Guid.NewGuid(), "Test User", 5)
        };

        _mockRepo.Setup(r => r.GetTopBorrowersAsync(startDate, endDate, limit))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetTopBorrowersAsync(startDate, endDate, limit);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _mockRepo.Verify(r => r.GetTopBorrowersAsync(startDate, endDate, limit), Times.Once);
    }

    [Fact]
    public async Task GetUserReadingPaceAsync_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var expectedResult = new UserReadingPaceSummaryDto(userId, "Test User", 42.5);

        _mockRepo.Setup(r => r.GetUserReadingPaceAsync(userId, bookId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetUserReadingPaceAsync(userId, bookId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _mockRepo.Verify(r => r.GetUserReadingPaceAsync(userId, bookId), Times.Once);
    }
}
