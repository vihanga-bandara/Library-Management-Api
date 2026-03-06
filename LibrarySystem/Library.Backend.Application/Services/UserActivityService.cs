using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly ILibraryAnalyticsRepository _libraryAnalyticsRepo;

        public UserActivityService(ILibraryAnalyticsRepository libraryAnalyticsRepo)
        {
            _libraryAnalyticsRepo = libraryAnalyticsRepo;
        }

        public Task<List<UserBorrowSummaryDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit)
        {
            return _libraryAnalyticsRepo.GetTopBorrowersAsync(startDate, endDate, limit);
        }

        public Task<UserReadingPaceSummaryDto?> GetUserReadingPaceAsync(Guid userId, Guid? bookId)
        {
            return _libraryAnalyticsRepo.GetUserReadingPaceAsync(userId, bookId);
        }
    }
}
