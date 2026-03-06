using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public interface IUserActivityService
    {
        Task<List<UserBorrowSummaryDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit);
        Task<UserReadingPaceSummaryDto?> GetUserReadingPaceAsync(Guid userId, Guid? bookId);
    }
}
