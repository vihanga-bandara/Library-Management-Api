using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Library.Backend.Grpc.Interceptors
{
    public class GlobalExceptionInterceptor : Interceptor
    {
        private readonly ILogger<GlobalExceptionInterceptor> _logger;

        public GlobalExceptionInterceptor(ILogger<GlobalExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                // Let the request flow to your gRPC service (and down to your App/Infrastructure layers)
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        private RpcException HandleException(Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during a gRPC call.");

            var status = new Status(StatusCode.Internal, "An unexpected error occurred on the server.");
            return new RpcException(status);
        }
    }
}