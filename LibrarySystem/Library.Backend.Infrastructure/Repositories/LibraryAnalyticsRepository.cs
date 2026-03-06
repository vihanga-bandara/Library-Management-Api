using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;
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
        public async Task<List<BorrowedBooksDto>> GetMostBorrowedBooksAsync(int limit)
        {
            return await _dbContext.Books.AsNoTracking()
                .Select(b => new BorrowedBooksDto(
                    b.Id, b.Title, b.LoanTransactions.Count()
                    ))
                .Where(b => b.BookCount > 0)
                .OrderByDescending(b => b.BookCount)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<UserBorrowSummaryDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit)
        {
            return await _dbContext.Users.AsNoTracking()
                .Select(u => new UserBorrowSummaryDto(
                    u.Id,
                    u.Name,
                    u.LoanTransactions
                        .Count(lt => lt.BorrowedAt >= startDate && lt.BorrowedAt <= endDate)
                        ))
                .Where(u => u.BorrowedCount > 0)
                .OrderByDescending(u => u.BorrowedCount)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<UserReadingPaceSummaryDto?> GetUserReadingPaceAsync(Guid userId, Guid? bookId)
        {
            // had to do it in repo due to Sqlite limitation and not having SQL functions support
            var user = await _dbContext.Users.AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new { u.Id, u.Name })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

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

            if (!readingData.Any())
            {
                return new UserReadingPaceSummaryDto(user.Id, user.Name, 0);
            }

            var averagePagesPerDay = readingData.Average(data =>
                (double)data.PageCount / Math.Max((data.ReturnedAt - data.BorrowedAt).TotalDays, 1.0)
            );

            return new UserReadingPaceSummaryDto(user.Id, user.Name, averagePagesPerDay);
        }

        public async Task<List<BorrowedBooksDto>> GetOtherBorrowedBooksAsync(Guid bookId, int limit)
        {
            var usersWhoBorrowedThisBook = await _dbContext.LoanTransactions
                .AsNoTracking()
                .Where(lt => lt.BookId == bookId)
                .Select(lt => lt.UserId)
                .Distinct()
                .ToListAsync();

            if (!usersWhoBorrowedThisBook.Any())
            {
                return new List<BorrowedBooksDto>();
            }

            var otherBorrowedBooks = await _dbContext.LoanTransactions
                .AsNoTracking()
                .Include(lt => lt.Book)
                .Where(lt => usersWhoBorrowedThisBook.Contains(lt.UserId) && lt.BookId != bookId)
                .ToListAsync();

            return otherBorrowedBooks
                .GroupBy(lt => new { lt.BookId, lt.Book!.Title })
                .Select(g => new BorrowedBooksDto(
                    g.Key.BookId,
                    g.Key.Title,
                    g.Count()
                ))
                .OrderByDescending(b => b.BookCount)
                .Take(limit)
                .ToList();
        }
    }
}
