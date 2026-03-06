using Library.Backend.Application.Interfaces;
using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public class InventoryInsightsService : IInventoryInsightsService
    {
        private readonly ILibraryAnalyticsRepository _analyticsRepo;

        public InventoryInsightsService(ILibraryAnalyticsRepository analyticsRepo)
        {
            _analyticsRepo = analyticsRepo;
        }
        public async Task<List<BorrowedBooksDto>> GetMostBorrowedBooksAsync(int limit)
        {
            return await _analyticsRepo.GetMostBorrowedBooksAsync(limit);
        }
    }
}
