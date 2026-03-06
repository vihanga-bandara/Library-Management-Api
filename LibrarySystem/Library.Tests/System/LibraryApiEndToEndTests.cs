using System.Net;
using FluentAssertions;

namespace Library.Tests.System;

[Trait("Category", "E2E")]
public class LibraryApiEndToEndTests
{
    private const string ApiBaseUrl = "http://localhost:5001";

    [Fact]
    public async Task GetMostBorrowedBooks_ShouldReturnSuccessStatusCode()
    {
        // Arrange
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };

        // Act
        var response = await client.GetAsync("/api/v1/InventoryInsights/most-borrowed-books?limit=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetTopBorrowers_ShouldReturnSuccessWithDateRange()
    {
        // Arrange
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var startDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        var url = $"/api/v1/UserActivity/top-borrower?startDate={startDate:O}&endDate={endDate:O}&limit=5";

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUserReadingPace_ShouldHandleValidUserId()
    {
        // Arrange
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        // Act
        var response = await client.GetAsync($"/api/v1/UserActivity/reading-pace/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOtherBorrowedBooks_ShouldReturnSuccessStatusCode()
    {
        // Arrange
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var bookId = Guid.Parse("b0000001-0000-0000-0000-000000000001");

        // Act
        var response = await client.GetAsync($"/api/v1/Recommendation/other-borrowed-books/{bookId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
