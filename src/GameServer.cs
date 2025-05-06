using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsteroidsServer.Src;

public class GameServer(ComputationLoop computationLoop)
{
    private readonly ComputationLoop computationLoop = computationLoop;
    private readonly TcpListener tcpListener = new(IPAddress.Parse("127.0.0.1"), 8080);
    private readonly Dictionary<string, Socket> sockets = [];

    public void Start()
    {

    }
}