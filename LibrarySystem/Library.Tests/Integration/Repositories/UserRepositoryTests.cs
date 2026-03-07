using Library.Backend.Application.Interfaces;
using Library.Backend.Infrastructure.Persistence;
using Library.Tests.Integration.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Library.Backend.Infrastructure;
using FluentAssertions;


namespace Library.Tests.Integration.Repositories
{
    public class UserRepositoryTests
    {
        private readonly LibraryDbContext _context;
        private readonly IUserRepository _repository;
        private readonly IServiceScope _scope;
        private readonly ServiceProvider _serviceProvider;

        public UserRepositoryTests()
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

            _repository = _scope.ServiceProvider.GetRequiredService<IUserRepository>();
        }

        [Fact]
        public async Task GetUserbyId_ShouldReturnNullForNonExistingUser()
        {
            // Arrange - Charlie is the user with no returned books
            var userWithNoReturns = Guid.Parse("11111314-1111-1111-1111-111111111111");

            // Act
            var result = await _repository.GetUserById(userWithNoReturns);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserbyId_ShouldReturnExistingUser()
        {
            var existingUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var existingUserName = "Alice";

            // Act
            var result = await _repository.GetUserById(existingUserId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingUserId);
            result.Name.Should().BeEquivalentTo(existingUserName);
        }
    }
}
