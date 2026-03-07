
using Grpc.Core;
using Library.Backend.Application.Services;
using Library.Shared.Contracts.Inventory.V1;

namespace Libary.Backend.Grpc.Services
{
    public class InventoryGrpcService : InventoryService.InventoryServiceBase
    {
        private readonly IInventoryInsightsService _inventoryInsightsService;

        public InventoryGrpcService(IInventoryInsightsService inventoryInsightsService)
        {
            _inventoryInsightsService = inventoryInsightsService;
        }
        public override async Task<GetMostBorrowedBooksResponse> GetMostBorrowedBooks(GetMostBorrowedBooksRequest request, ServerCallContext context)
        {
            var books = await _inventoryInsightsService.GetMostBorrowedBooksAsync(request.Limit);

            var response = new GetMostBorrowedBooksResponse();
            response.BorrowedBooks.AddRange(books.Select(b => new BorrowedBook
            {
                Id = b.Id.ToString(),
                Title = b.Title,
                BookCount = b.BookCount,
            }));

            return response;
        }
    }
}
