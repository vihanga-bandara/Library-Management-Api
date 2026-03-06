using Grpc.Core;
using Library.Backend.Application.Services;
using Library.Shared.Contracts.Recommendation.V1;
using RecommendationProto = Library.Shared.Contracts.Recommendation.V1;

namespace Libary.Backend.Grpc.Services
{
    public class RecommendationGrpcService : RecommendationProto.RecommendationService.RecommendationServiceBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationGrpcService(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public override async Task<GetOtherBorrowedBooksResponse> GetOtherBorrowedBooks(GetOtherBorrowedBooksRequest request, ServerCallContext context)
        {
            var bookId = Guid.Parse(request.BookId);
            var recommendations = await _recommendationService.GetOtherBorrowedBooksAsync(bookId, limit: 10);

            var response = new GetOtherBorrowedBooksResponse();
            response.RecommendedBooks.AddRange(recommendations.Select(book => new RecommendedBook
            {
                BookId = book.Id.ToString(),
                Title = book.Title
            }));

            return response;
        }
    }
}
