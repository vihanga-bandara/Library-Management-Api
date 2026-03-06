using Library.Shared.Contracts.UserActivity.V1;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserActivityController : ControllerBase
    {
        private readonly UserActivityService.UserActivityServiceClient _grpcClient;

        public UserActivityController(UserActivityService.UserActivityServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpGet("top-borrower")]
        public async Task<IActionResult> GetTopBorrowers([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int limit = 10)
        {
            var response = await _grpcClient.GetTopBorrowersAsync(new GetTopBorrowersRequest
            {
                StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(startDate),
                EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(endDate),
                Limit = limit
            });

            return Ok(response);
        }

        [HttpGet("reading-pace/{userId}")]
        public async Task<IActionResult> GetUserReadingPace(string userId, [FromQuery] string? bookId = null)
        {
            var response = await _grpcClient.GetUserReadingPaceAsync(new GetUserReadingPaceRequest
            {
                UserId = userId,
                BookId = bookId ?? string.Empty
            });

            return Ok(response);
        }
    }
}
