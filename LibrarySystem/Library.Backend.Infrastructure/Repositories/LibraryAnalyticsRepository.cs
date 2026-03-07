using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;
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

        public async Task<List<BorrowedBooksDto>> GetMostBorrowedBooksAsync(int limit)
        {
            return await _dbContext.LoanTransactions
                .AsNoTracking()
                .GroupBy(lt => new { lt.BookId, lt.Book!.Title })
                .OrderByDescending(g => g.Count())
                .Take(limit)
                .Select(g => new BorrowedBooksDto(
                    g.Key.BookId,
                    g.Key.Title,
                    g.Count()
                ))
                .ToListAsync();
        }

        public async Task<List<UserBorrowSummaryDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit)
        {
            return await _dbContext.LoanTransactions
                .AsNoTracking()
                .Where(lt => lt.BorrowedAt >= startDate && lt.BorrowedAt <= endDate)
                .GroupBy(lt => new { lt.UserId, lt.User!.Name })
                .OrderByDescending(g => g.Count())
                .Take(limit)
                .Select(g => new UserBorrowSummaryDto(
                    g.Key.UserId,
                    g.Key.Name,
                    g.Count()
                ))
                .ToListAsync();
        }

        public async Task<UserReadingPaceSummaryDto?> GetUserReadingPaceAsync(Guid userId, string name, Guid? bookId)
        {
            var query = _dbContext.LoanTransactions
                .AsNoTracking()
                .Where(lt => lt.UserId == userId && lt.ReturnedAt != null);

            if (bookId.HasValue)
            {
                query = query.Where(lt => lt.BookId == bookId.Value);
            }

            var readingData = await query
                .Select(lt => new
                {
                    lt.BorrowedAt,
                    ReturnedAt = lt.ReturnedAt!.Value,
                    lt.Book!.PageCount
                })
                .ToListAsync();

            if (readingData.Count == 0)
            {
                return new UserReadingPaceSummaryDto(userId, name, 0);
            }

            var averagePagesPerDay = readingData.Average(d =>
                (double)d.PageCount / Math.Max((d.ReturnedAt - d.BorrowedAt).TotalDays, 1.0));

            return new UserReadingPaceSummaryDto(userId, name, averagePagesPerDay);
        }

        public async Task<List<BorrowedBooksDto>> GetOtherBorrowedBooksAsync(Guid bookId, int limit)
        {
            var usersWhoBorrowedBook = _dbContext.LoanTransactions
                .Where(lt => lt.BookId == bookId)
                .Select(lt => lt.UserId);

            return await _dbContext.LoanTransactions
                .AsNoTracking()
                .Where(lt => usersWhoBorrowedBook.Contains(lt.UserId) && lt.BookId != bookId)
                .GroupBy(lt => new { lt.BookId, lt.Book!.Title })
                .OrderByDescending(g => g.Count())
                .Take(limit)
                .Select(g => new BorrowedBooksDto(
                    g.Key.BookId,
                    g.Key.Title,
                    g.Count()
                ))
                .ToListAsync();
        }
    }
}
