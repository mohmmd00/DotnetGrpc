using Common.Proto;
using Microsoft.Extensions.DependencyInjection;

namespace MPS.Infrastructure.Grpc.Client
{
    public static class ServiceCollectionExtension
    {
        public static void AddGrpcSdk(this IServiceCollection services)
        {
            services.AddGrpcClient<MessageExchange.MessageExchangeClient>
            (client =>
            {
                client.Address = new Uri("https://localhost:7085");
            });
        }
    }
}
