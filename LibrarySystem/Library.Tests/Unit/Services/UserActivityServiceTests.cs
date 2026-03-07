using FluentAssertions;
using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;
using Library.Backend.Application.Services;
using Library.Backend.Domain.Entities;
using Moq;

namespace Library.Tests.Unit.Services;

public class UserActivityServiceTests
{
    private readonly Mock<ILibraryAnalyticsRepository> _mockLibAnalyticsRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly UserActivityService _service;

    public UserActivityServiceTests()
    {
        _mockLibAnalyticsRepo = new Mock<ILibraryAnalyticsRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _service = new UserActivityService(_mockLibAnalyticsRepo.Object, _mockUserRepo.Object);
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

        _mockLibAnalyticsRepo.Setup(r => r.GetTopBorrowersAsync(startDate, endDate, limit))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetTopBorrowersAsync(startDate, endDate, limit);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _mockLibAnalyticsRepo.Verify(r => r.GetTopBorrowersAsync(startDate, endDate, limit), Times.Once);
    }

    [Fact]
    public async Task GetUserReadingPaceAsync_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Name = "Test User" };

        var bookId = Guid.NewGuid();
        var expectedResult = new UserReadingPaceSummaryDto(user.Id, user.Name, 42.5);

        _mockUserRepo.Setup(r => r.GetUserById(user.Id))
            .ReturnsAsync(user);

        _mockLibAnalyticsRepo.Setup(r => r.GetUserReadingPaceAsync(user.Id, user.Name, bookId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.GetUserReadingPaceAsync(user.Id, bookId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
        _mockUserRepo.Verify(r => r.GetUserById(user.Id), Times.Once);
        _mockLibAnalyticsRepo.Verify(r => r.GetUserReadingPaceAsync(user.Id, user.Name, bookId), Times.Once);
    }
}
