using Library.Backend.Application.Interfaces;
using Library.Backend.Domain.Entities;
using Library.Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Backend.Infrastructure.Repositories
{
    internal class LibraryAnalyticsRepository : ILibraryAnalyticsRepository
    {
        private readonly LibraryDbContext _dbContext;

        public LibraryAnalyticsRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Book>> GetMostBorrowedBooksAsync(int limit)
        {
            return await _dbContext.LoanTransactions.AsNoTracking()
                .GroupBy(lt => lt.BookId)
                .OrderByDescending(g => g.Count())
                // we could have a max query amount, but I did not really do that for the purpose of assignment
                .Take(limit)
                .Select(g => g.Key)
                .Join(_dbContext.Books,
                    bookId => bookId,
                    book => book.Id,
                    (bookId, book) => book)
                .ToListAsync();
        }
    }
}
