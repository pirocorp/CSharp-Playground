namespace BlockExplorer
{
    using Grpc.Net.Client;

    using static GrpcService.BChainService;

    public static class Program
    {
        public static void Main()
        {
            var serverAddress = "https://localhost:5001";
            var channel = GrpcChannel.ForAddress(serverAddress);
            var blockChainService = new BChainServiceClient(channel);

            var consoleExplorer = new ConsoleExplorer(blockChainService);
            consoleExplorer.Run();
        }
    }
}
