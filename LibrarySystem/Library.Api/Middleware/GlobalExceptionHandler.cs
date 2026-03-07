using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is RpcException rpcException)
            {
                var statusCode = rpcException.StatusCode switch
                {
                    StatusCode.NotFound => StatusCodes.Status404NotFound,
                    StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
                    StatusCode.AlreadyExists => StatusCodes.Status409Conflict,
                    StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
                    StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
                    StatusCode.ResourceExhausted => StatusCodes.Status429TooManyRequests,
                    StatusCode.FailedPrecondition => StatusCodes.Status412PreconditionFailed,
                    StatusCode.Aborted => StatusCodes.Status409Conflict,
                    StatusCode.OutOfRange => StatusCodes.Status400BadRequest,
                    StatusCode.Unimplemented => StatusCodes.Status501NotImplemented,
                    StatusCode.Unavailable => StatusCodes.Status503ServiceUnavailable,
                    StatusCode.DeadlineExceeded => StatusCodes.Status408RequestTimeout,
                    _ => StatusCodes.Status500InternalServerError
                };

                var problemDetails = new ProblemDetails
                {
                    Instance = httpContext.Request.Path,
                    Status = statusCode,
                    Title = rpcException.StatusCode.ToString(),
                    Detail = rpcException.Status.Detail
                };

                _logger.LogError(rpcException, "gRPC error occurred: {StatusCode}", rpcException.StatusCode);
                httpContext.Response.StatusCode = problemDetails.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }

            var defaultProblemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path,
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred. Please try again later."
            };

            _logger.LogError(exception, "An unexpected system error occurred.");
            httpContext.Response.StatusCode = defaultProblemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(defaultProblemDetails, cancellationToken);

            return true;
        }
    }
}
