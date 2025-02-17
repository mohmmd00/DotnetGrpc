using Grpc.Net.Client;
using MPS.Infrastructure.Client;
using MRS.Infrastructure.GRPC.Services;

namespace MPS.Insfrastructure.Client
{
    public class Program
    {

        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7104");
            var client = new ProcessInputMessage.ProcessInputMessageClient(channel);


            Console.WriteLine("Hello, World!");
        }
    }
}
