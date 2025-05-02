using AsteroidsServer.Src;

class Program
{
    static void Main(string[] args)
    {
        GameServer server = new(new Spawner(), new GameState());
        server.Start();
    }
}
