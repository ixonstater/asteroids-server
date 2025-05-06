using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsteroidsServer.Src;

public class GameServer(ComputationLoop computationLoop)
{
    private readonly ComputationLoop computationLoop = computationLoop;
    private readonly TcpListener tcpListener = new(IPAddress.Parse("127.0.0.1"), 8080);

    public void Start()
    {
        tcpListener.Start();

        while (true)
        {
            // Each time a new socket is opened we should spawn a new thread to listen for data from it.
            Socket socket = tcpListener.AcceptSocket();
            Console.WriteLine("Accepted");
            socket.Disconnect(true);
        }
    }
}