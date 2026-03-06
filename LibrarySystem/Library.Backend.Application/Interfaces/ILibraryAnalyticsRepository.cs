using Library.Backend.Domain.Entities;

namespace Library.Backend.Application.Interfaces
{
    public interface ILibraryAnalyticsRepository
    {
        Task<List<Book>> GetMostBorrowedBooksAsync(int limit);
    }
}
