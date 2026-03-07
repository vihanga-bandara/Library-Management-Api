using System.Net;
using FluentAssertions;

namespace Library.Tests.System;

[Trait("Category", "E2E")]
public class LibraryApiEndToEndTests
{
    private const string ApiBaseUrl = "http://localhost:5194";

    private const string E2ETestBookId = "b0000001-0000-0000-0000-000000000001";
    private const string E2ETestUserId = "11111111-1111-1111-1111-111111111111";

    [Fact]
    public async Task MostBorrowedBooks_ShouldRespond()
    {
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var response = await client.GetAsync("/api/v1/InventoryInsights/most-borrowed-books?limit=5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TopBorrowers_ShouldRespond()
    {
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var response = await client.GetAsync("/api/v1/UserActivity/top-borrower?startDate=2024-01-01&endDate=2024-12-31&limit=5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UserReadingPace_WithSeededUser_ShouldRespond()
    {
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var response = await client.GetAsync($"/api/v1/UserActivity/reading-pace/{E2ETestUserId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task OtherBorrowedBooks_WithSeededBook_ShouldRespond()
    {
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var response = await client.GetAsync($"/api/v1/Recommendation/other-borrowed-books/{E2ETestBookId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UserReadingPace_WithSeededUserAndBook_ShouldRespond()
    {
        using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
        var response = await client.GetAsync($"/api/v1/UserActivity/reading-pace/{E2ETestUserId}?bookId={E2ETestBookId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
