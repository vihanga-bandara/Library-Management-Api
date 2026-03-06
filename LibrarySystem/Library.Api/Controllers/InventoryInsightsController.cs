using Library.Shared.Contracts.Inventory.V1;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InventoryInsightsController : ControllerBase
    {
        private readonly InventoryService.InventoryServiceClient _grpcClient;

        public InventoryInsightsController(InventoryService.InventoryServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpGet("most-borrowed-books")]
        public async Task<IActionResult> GetMostBorrowedBooks([FromQuery] int limit = 10)
        {
            var response = await _grpcClient.GetMostBorrowedBooksAsync(new GetMostBorrowedBooksRequest
            {
                Limit = limit
            });

            return Ok(response.BorrowedBooks);
        }
    }
}
