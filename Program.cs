using AsteroidsServer.Src;
using AsteroidsServer.Test;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "test")
        {
            RunTests.Run();
            return;
        }

        GameStateMessageProcessor gameStateMessageProcessor = new();
        GameServer server = new(new(new(), new(), gameStateMessageProcessor), new(new()), gameStateMessageProcessor);
        server.Start();
    }
}
