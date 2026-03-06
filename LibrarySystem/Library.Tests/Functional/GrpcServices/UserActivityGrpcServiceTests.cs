using FluentAssertions;
using Grpc.Core;
using Libary.Backend.Grpc.Services;
using Library.Backend.Application.Models;
using Library.Backend.Application.Services;
using Library.Shared.Contracts.UserActivity.V1;
using Moq;

namespace Library.Tests.Functional.GrpcServices;

public class UserActivityGrpcServiceTests
{
    private readonly Mock<IUserActivityService> _mockService;
    private readonly UserActivityGrpcService _grpcService;

    public UserActivityGrpcServiceTests()
    {
        _mockService = new Mock<IUserActivityService>();
        _grpcService = new UserActivityGrpcService(_mockService.Object);
    }

    [Fact]
    public async Task GetTopBorrowers_ShouldMapDtosToProtoCorrectly()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        var userId = Guid.NewGuid();

        var request = new GetTopBorrowersRequest
        {
            StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(startDate),
            EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(endDate),
            Limit = 10
        };

        var users = new List<UserBorrowSummaryDto>
        {
            new(userId, "Alice", 5)
        };

        _mockService.Setup(s => s.GetTopBorrowersAsync(startDate, endDate, 10))
            .ReturnsAsync(users);

        // Act
        var response = await _grpcService.GetTopBorrowers(request, TestServerCallContext.Create());

        // Assert
        response.TopBorrowers.Should().HaveCount(1);
        response.TopBorrowers[0].UserId.Should().Be(userId.ToString());
        response.TopBorrowers[0].Name.Should().Be("Alice");
        response.TopBorrowers[0].TotalBorrowedBooks.Should().Be(5);
    }

    [Fact]
    public async Task GetUserReadingPace_ShouldReturnPaceWhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new GetUserReadingPaceRequest
        {
            UserId = userId.ToString(),
            BookId = string.Empty
        };

        var readingPace = new UserReadingPaceSummaryDto(userId, "Alice", 42.7);
        _mockService.Setup(s => s.GetUserReadingPaceAsync(userId, null))
            .ReturnsAsync(readingPace);

        // Act
        var response = await _grpcService.GetUserReadingPace(request, TestServerCallContext.Create());

        // Assert
        response.UserId.Should().Be(userId.ToString());
        response.PagesPerDay.Should().Be(43);
    }

    [Fact]
    public async Task GetUserReadingPace_ShouldThrowRpcExceptionWhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new GetUserReadingPaceRequest
        {
            UserId = userId.ToString(),
            BookId = string.Empty
        };

        _mockService.Setup(s => s.GetUserReadingPaceAsync(userId, null))
            .ReturnsAsync((UserReadingPaceSummaryDto?)null);

        // Act
        var act = async () => await _grpcService.GetUserReadingPace(request, TestServerCallContext.Create());

        // Assert
        await act.Should().ThrowAsync<RpcException>()
            .Where(e => e.StatusCode == StatusCode.NotFound);
    }
}

public class TestServerCallContext : ServerCallContext
{
    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders) => Task.CompletedTask;
    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options) => null!;
    protected override string MethodCore => "TestMethod";
    protected override string HostCore => "localhost";
    protected override string PeerCore => "peer";
    protected override DateTime DeadlineCore => DateTime.MaxValue;
    protected override Metadata RequestHeadersCore => new();
    protected override CancellationToken CancellationTokenCore => CancellationToken.None;
    protected override Metadata ResponseTrailersCore => new();
    protected override Status StatusCore { get; set; }
    protected override WriteOptions? WriteOptionsCore { get; set; }
    protected override AuthContext AuthContextCore => null!;

    public static ServerCallContext Create() => new TestServerCallContext();
}
