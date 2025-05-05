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
        GameServer server = new(new Spawner(), new GameState());
        server.Start();
    }
}
