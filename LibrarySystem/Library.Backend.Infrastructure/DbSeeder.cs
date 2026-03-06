using Library.Backend.Domain.Entities;
using Library.Backend.Infrastructure.Persistence;

namespace Library.Backend.Infrastructure
{
    public class DbSeeder
    {
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
                    Id = Guid.NewGuid(),
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
                    Id = Guid.NewGuid(),
                    Name = $"User {i}"
                };
                users.Add(user);
            }
            dbContext.Users.AddRange(users);

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

            dbContext.SaveChanges();
        }
    }
}
