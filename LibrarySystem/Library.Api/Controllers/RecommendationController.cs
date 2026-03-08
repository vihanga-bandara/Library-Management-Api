using Library.Api.Models;
using Library.Shared.Contracts.Recommendation.V1;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationService.RecommendationServiceClient _grpcClient;

        public RecommendationController(RecommendationService.RecommendationServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpGet("other-borrowed-books/{bookId:guid}")]
        public async Task<IActionResult> GetOtherBorrowedBooks(Guid bookId, [FromQuery] int limit = QueryConstants.DefaultLimit)
        {
            var response = await _grpcClient.GetOtherBorrowedBooksAsync(
                new GetOtherBorrowedBooksRequest
                {
                    BookId = bookId.ToString(),
                    Limit = limit
                });

            return Ok(response);
        }
    }
}
