using Library.Backend.Application.Models;

namespace Library.Backend.Application.Services
{
    public interface IRecommendationService
    {
        Task<List<BorrowedBooksDto>> GetOtherBorrowedBooksAsync(Guid bookId, int limit);
    }
}
