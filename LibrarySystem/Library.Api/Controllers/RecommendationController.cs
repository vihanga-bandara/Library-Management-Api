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

        [HttpGet("other-borrowed-books/{bookId}")]
        public async Task<IActionResult> GetOtherBorrowedBooks(string bookId)
        {
            var response = await _grpcClient.GetOtherBorrowedBooksAsync(new GetOtherBorrowedBooksRequest
            {
                BookId = bookId
            });

            return Ok(response);
        }
    }
}
