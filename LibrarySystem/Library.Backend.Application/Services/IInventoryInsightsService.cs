using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public interface IInventoryInsightsService
    {
        Task<List<BorrowedBookDto>> GetMostBorrowedBooksAsync(int limit);
    }
}
