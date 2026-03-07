using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly ILibraryAnalyticsRepository _libraryAnalyticsRepo;
        private readonly IUserRepository _userRepository;

        public UserActivityService(ILibraryAnalyticsRepository libraryAnalyticsRepo, IUserRepository userRepository)
        {
            _libraryAnalyticsRepo = libraryAnalyticsRepo;
            _userRepository = userRepository;
        }

        public async Task<List<UserBorrowSummaryDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit)
        {
            return await _libraryAnalyticsRepo.GetTopBorrowersAsync(startDate, endDate, limit);
        }

        public async Task<UserReadingPaceSummaryDto?> GetUserReadingPaceAsync(Guid userId, Guid? bookId)
        {
            // check whether user exists
            var user = await _userRepository.GetUserById(userId);
            
            if (user == null) { return null; }

            return await _libraryAnalyticsRepo.GetUserReadingPaceAsync(user.Id, user.Name, bookId);
        }
    }
}
