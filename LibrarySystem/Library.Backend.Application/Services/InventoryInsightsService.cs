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
        public async Task<List<BorrowedBookDto>> GetMostBorrowedBooksAsync(int limit)
        {
            var mostBorrowedBooks = await _analyticsRepo.GetMostBorrowedBooksAsync(limit);

            return mostBorrowedBooks.Select(b => new BorrowedBookDto(b.Id, b.Title)).ToList();
        }
    }
}
