using FluentAssertions;
using Library.Backend.Application.Interfaces;
using Library.Backend.Infrastructure;
using Library.Backend.Infrastructure.Persistence;
using Library.Tests.Integration.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Tests.Integration.Repositories;

public class LibraryAnalyticsRepositoryTests : IDisposable
{
    private readonly LibraryDbContext _context;
    private readonly ILibraryAnalyticsRepository _repository;
    private readonly IServiceScope _scope;
    private readonly ServiceProvider _serviceProvider;

    public LibraryAnalyticsRepositoryTests()
    {
        var connectionString = TestDbContextFactory.CreateConnectionString();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LibraryDb"] = connectionString
            })
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructure(configuration);

        _serviceProvider = services.BuildServiceProvider();
        _scope = _serviceProvider.CreateScope();

        _context = _scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
        _context.Database.EnsureCreated();
        TestDbContextFactory.SeedTestData(_context);

        _repository = _scope.ServiceProvider.GetRequiredService<ILibraryAnalyticsRepository>();
    }

    [Fact]
    public async Task GetMostBorrowedBooksAsync_ShouldReturnBooksOrderedByBorrowCount()
    {
        // Act
        var result = await _repository.GetMostBorrowedBooksAsync(10);

        // Assert
        result.Should().NotBeEmpty();
        result.First().BookCount.Should().Be(3);
        result.Should().BeInDescendingOrder(b => b.BookCount);
    }

    [Fact]
    public async Task GetMostBorrowedBooksAsync_ShouldRespectLimitParameter()
    {
        // Act
        var result = await _repository.GetMostBorrowedBooksAsync(1);

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetTopBorrowersAsync_ShouldReturnUsersInDateRange()
    {
        // Arrange
        var startDate = new DateTime(2026, 1, 1);
        var endDate = new DateTime(2026, 1, 31);

        // Act
        var result = await _repository.GetTopBorrowersAsync(startDate, endDate, 10);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(u => u.BorrowedCount.Should().BeGreaterThan(0));
    }

    [Fact]
    public async Task GetUserReadingPaceAsync_ShouldCalculateAveragePagesPerDay()
    {
        // Arrange
        var user = _context.Users.First();

        // Act
        var result = await _repository.GetUserReadingPaceAsync(user.Id, null);

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be(user.Id);
        result.PagesPerDay.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetUserReadingPaceAsync_ShouldReturnZeroForUserWithNoReturnedBooks()
    {
        // Arrange - Charlie is the user with no returned books
        var userWithNoReturns = _context.Users.First(u => u.Name == "Charlie");

        // Act
        var result = await _repository.GetUserReadingPaceAsync(userWithNoReturns.Id, null);

        // Assert
        result.Should().NotBeNull();
        result!.PagesPerDay.Should().Be(0);
    }

    [Fact]
    public async Task GetUserReadingPaceAsync_ShouldReturnNullForNonExistentUser()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act
        var result = await _repository.GetUserReadingPaceAsync(nonExistentUserId, null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOtherBorrowedBooksAsync_ShouldReturnCoBorrowedBooks()
    {
        // Arrange
        var book = _context.Books.First();

        // Act
        var result = await _repository.GetOtherBorrowedBooksAsync(book.Id, 10);

        // Assert
        result.Should().NotContain(b => b.Id == book.Id);
    }

    [Fact]
    public async Task GetOtherBorrowedBooksAsync_ShouldReturnEmptyForBookWithNoBorrowers()
    {
        // Arrange
        var unborrowed = _context.Books.Add(new Backend.Domain.Entities.Book 
        { 
            Id = Guid.NewGuid(), 
            Title = "Unborrowed Book", 
            PageCount = 100 
        }).Entity;
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOtherBorrowedBooksAsync(unborrowed.Id, 10);

        // Assert
        result.Should().BeEmpty();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _scope.Dispose();
        _serviceProvider.Dispose();
    }
}
