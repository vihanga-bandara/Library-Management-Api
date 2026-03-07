using Library.Backend.Domain.Entities;
using Library.Backend.Infrastructure.Persistence;

namespace Library.Tests.Integration.Helpers;

public static class TestDbContextFactory
{
    private const string ConnectionStringTemplate =
        "Server=(localdb)\\mssqllocaldb;Database={0};Trusted_Connection=True;MultipleActiveResultSets=true";

    public static string CreateConnectionString()
    {
        return string.Format(ConnectionStringTemplate, $"Library_Tests_{Guid.NewGuid():N}");
    }

    public static void SeedTestData(LibraryDbContext context)
    {
        var book1 = new Book { Id = Guid.NewGuid(), Title = "Book One", PageCount = 300 };
        var book2 = new Book { Id = Guid.NewGuid(), Title = "Book Two", PageCount = 200 };
        var book3 = new Book { Id = Guid.NewGuid(), Title = "Book Three", PageCount = 400 };

        var user1 = new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Alice" };
        var user2 = new User { Id = Guid.Parse("12111111-1111-1111-1111-111111111111"), Name = "Bob" };
        var user3 = new User { Id = Guid.Parse("12311111-1111-1111-1111-111111111111"), Name = "Charlie" };

        context.Books.AddRange(book1, book2, book3);
        context.Users.AddRange(user1, user2, user3);
        context.SaveChanges();

        var baseDate = new DateTime(2024, 1, 1);

        context.LoanTransactions.AddRange(
            // Alice - has returned books
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book1.Id, UserId = user1.Id, BorrowedAt = baseDate, ReturnedAt = baseDate.AddDays(10) },
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book2.Id, UserId = user1.Id, BorrowedAt = baseDate, ReturnedAt = baseDate.AddDays(5) },
            // Bob - has returned books
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book1.Id, UserId = user2.Id, BorrowedAt = baseDate.AddDays(15), ReturnedAt = baseDate.AddDays(20) },
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book3.Id, UserId = user2.Id, BorrowedAt = baseDate.AddDays(25), ReturnedAt = baseDate.AddDays(35) },
            // Charlie - only has unreturned books (for testing edge case)
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book1.Id, UserId = user3.Id, BorrowedAt = baseDate.AddDays(30), ReturnedAt = null },
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book2.Id, UserId = user3.Id, BorrowedAt = baseDate.AddDays(20), ReturnedAt = null },
            // Additional transactions for 2026 date range test
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book2.Id, UserId = user1.Id, BorrowedAt = new DateTime(2026, 1, 5), ReturnedAt = new DateTime(2026, 1, 15) },
            new LoanTransaction { Id = Guid.NewGuid(), BookId = book3.Id, UserId = user2.Id, BorrowedAt = new DateTime(2026, 1, 10), ReturnedAt = new DateTime(2026, 1, 20) }
        );

        context.SaveChanges();
    }
}
