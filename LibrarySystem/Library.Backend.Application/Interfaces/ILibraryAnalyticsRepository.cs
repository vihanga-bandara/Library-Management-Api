using Library.Backend.Application.Models;

namespace Library.Backend.Application.Interfaces
{
    public interface ILibraryAnalyticsRepository
    {
        Task<List<BorrowedBooksDto>> GetMostBorrowedBooksAsync(int limit);
        Task<List<UserBorrowSummaryDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit);
        Task<UserReadingPaceSummaryDto?> GetUserReadingPaceAsync(Guid userId, Guid? bookId);
        Task<List<BorrowedBooksDto>> GetOtherBorrowedBooksAsync(Guid bookId, int limit);
    }
}
