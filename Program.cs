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
        GameState gameState = new();
        GameServer server = new(new(new(), gameState, gameStateMessageProcessor), new(gameState), gameStateMessageProcessor);
        server.Start();
    }
}
