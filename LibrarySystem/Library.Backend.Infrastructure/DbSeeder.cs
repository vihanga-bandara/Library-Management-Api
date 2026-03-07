using Library.Backend.Domain.Entities;
using Library.Backend.Infrastructure.Persistence;

namespace Library.Backend.Infrastructure
{
    public class DbSeeder
    {
        public static readonly Guid[] SeedUserIds = 
        [
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Guid.Parse("66666666-6666-6666-6666-666666666666"),
            Guid.Parse("77777777-7777-7777-7777-777777777777"),
            Guid.Parse("88888888-8888-8888-8888-888888888888"),
            Guid.Parse("99999999-9999-9999-9999-999999999999"),
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
        ];

        public static readonly Guid[] SeedBookIds = 
        [
            Guid.Parse("b0000001-0000-0000-0000-000000000001"),
            Guid.Parse("b0000002-0000-0000-0000-000000000002"),
            Guid.Parse("b0000003-0000-0000-0000-000000000003"),
            Guid.Parse("b0000004-0000-0000-0000-000000000004"),
            Guid.Parse("b0000005-0000-0000-0000-000000000005")
        ];

        public static void Initialize(LibraryDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.Books.Any()) return;

            var rand = new Random();
            int numberOfBooksToCreate = 1000;
            int numberOfUsersToCreate = 10;
            int numberOfLoanTransactionsToCreate = 10000;

            var books = new List<Book>();
            for (int i = 1; i <= numberOfBooksToCreate; i++)
            {
                var book = new Book
                {
                    Id = i <= SeedBookIds.Length ? SeedBookIds[i - 1] : Guid.NewGuid(),
                    Title = $"Book Title {i}",
                    PageCount = rand.Next(100, 2001)
                };
                books.Add(book);
            }
            dbContext.Books.AddRange(books);

            var users = new List<User>();
            for (int i = 1; i <= numberOfUsersToCreate; i++)
            {
                var user = new User
                {
                    Id = SeedUserIds[i - 1],
                    Name = $"User {i}"
                };
                users.Add(user);
            }
            dbContext.Users.AddRange(users);

            // Create specific books for E2E testing
            var e2eTestBook = new Book
            {
                Id = SeedBookIds[0], // b0000001-0000-0000-0000-000000000001
                Title = "E2E Test Book - Most Borrowed",
                PageCount = 500
            };

            // Ensure this specific book exists (it should already from the loop above, but this guarantees it)
            var existingE2eBook = books.FirstOrDefault(b => b.Id == SeedBookIds[0]);
            if (existingE2eBook != null)
            {
                existingE2eBook.Title = "E2E Test Book - Most Borrowed";
                existingE2eBook.PageCount = 500;
            }

            // Create loan transactions
            for (int i = 0; i < numberOfLoanTransactionsToCreate; i++)
            {
                var randomBook = books[rand.Next(books.Count)];
                var randomUser = users[rand.Next(users.Count)];

                dbContext.LoanTransactions.Add(new LoanTransaction
                {
                    Id = Guid.NewGuid(),
                    BookId = randomBook.Id,
                    UserId = randomUser.Id,
                    BorrowedAt = DateTime.UtcNow.AddDays(-rand.Next(1, 365)),
                    ReturnedAt = rand.Next(0, 2) == 0 ? null : DateTime.UtcNow.AddDays(-rand.Next(0, 30))
                });
            }

            // Ensure first user has some returned books for testing
            var firstUser = users[0];
            for (int i = 0; i < 5; i++)
            {
                var randomBook = books[rand.Next(Math.Min(10, books.Count))];
                dbContext.LoanTransactions.Add(new LoanTransaction
                {
                    Id = Guid.NewGuid(),
                    BookId = randomBook.Id,
                    UserId = firstUser.Id,
                    BorrowedAt = DateTime.UtcNow.AddDays(-rand.Next(30, 90)),
                    ReturnedAt = DateTime.UtcNow.AddDays(-rand.Next(1, 29))
                });
            }

            // Create specific data for E2E test consistency
            // Ensure the first test book has multiple borrows to guarantee it shows in most-borrowed
            for (int i = 0; i < 15; i++)
            {
                dbContext.LoanTransactions.Add(new LoanTransaction
                {
                    Id = Guid.NewGuid(),
                    BookId = SeedBookIds[0], // E2E Test Book
                    UserId = users[rand.Next(users.Count)].Id,
                    BorrowedAt = DateTime.UtcNow.AddDays(-rand.Next(30, 180)),
                    ReturnedAt = rand.Next(0, 2) == 0 ? null : DateTime.UtcNow.AddDays(-rand.Next(1, 15))
                });
            }

            // Ensure first user (used in E2E tests) has borrowing activity in a recent date range
            var now = DateTime.UtcNow;
            for (int i = 0; i < 3; i++)
            {
                dbContext.LoanTransactions.Add(new LoanTransaction
                {
                    Id = Guid.NewGuid(),
                    BookId = books[rand.Next(5, Math.Min(15, books.Count))].Id,
                    UserId = SeedUserIds[0],
                    BorrowedAt = new DateTime(2024, rand.Next(1, 13), rand.Next(1, 28), 0, 0, 0, DateTimeKind.Utc),
                    ReturnedAt = null
                });
            }

            dbContext.SaveChanges();
        }
    }
}
