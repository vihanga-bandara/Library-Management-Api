using Grpc.Core;
using Library.Backend.Application.Services;
using Library.Shared.Contracts.UserActivity.V1;
using UserActivityProto = Library.Shared.Contracts.UserActivity.V1;

namespace Libary.Backend.Grpc.Services
{
    public class UserActivityGrpcService : UserActivityProto.UserActivityService.UserActivityServiceBase
    {
        public readonly IUserActivityService _userActivityService;
        public UserActivityGrpcService(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }

        public override async Task<GetTopBorrowersResponse> GetTopBorrowers(GetTopBorrowersRequest request, ServerCallContext context)
        {
            var users = await _userActivityService.GetTopBorrowersAsync(request.StartDate.ToDateTime(), request.EndDate.ToDateTime(), request.Limit);

            var response = new GetTopBorrowersResponse();
            response.TopBorrowers.AddRange(users.Select(user => new UserSummary
            {
                UserId = user.UserId.ToString(),
                Name = user.Name,
                TotalBorrowedBooks = user.BorrowedCount
            }));

            return response;
        }

        public override async Task<GetUserReadingPaceResponse> GetUserReadingPace(GetUserReadingPaceRequest request, ServerCallContext context)
        {
            var userId = Guid.Parse(request.UserId);
            Guid? bookId = string.IsNullOrEmpty(request.BookId) ? null : Guid.Parse(request.BookId);

            var readingPace = await _userActivityService.GetUserReadingPaceAsync(userId, bookId);

            if (readingPace == null)
            {
                // can probably make this a bad request, depends on who is calling (to avoid exposing such data)
                throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {userId} not found"));
            }

            return new GetUserReadingPaceResponse
            {
                UserId = readingPace.UserId.ToString(),
                BookId = request.BookId,
                PagesPerDay = (int)Math.Round(readingPace.pagesPerDay)
            };
        }
    }
}
