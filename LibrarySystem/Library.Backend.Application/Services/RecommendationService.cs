using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ILibraryAnalyticsRepository _analyticsRepo;

        public RecommendationService(ILibraryAnalyticsRepository analyticsRepo)
        {
            _analyticsRepo = analyticsRepo;
        }

        public async Task<List<BorrowedBooksDto>> GetOtherBorrowedBooksAsync(Guid bookId, int limit)
        {
            return await _analyticsRepo.GetOtherBorrowedBooksAsync(bookId, limit);
        }
    }
}
