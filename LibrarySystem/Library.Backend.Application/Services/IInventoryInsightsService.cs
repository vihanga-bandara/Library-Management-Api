using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public interface IInventoryInsightsService
    {
        Task<List<BorrowedBooksDto>> GetMostBorrowedBooksAsync(int limit);
    }
}
