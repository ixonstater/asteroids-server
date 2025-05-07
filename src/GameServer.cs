using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsteroidsServer.Src;

public class GameServer(ComputationLoop computationLoop)
{
    private readonly ComputationLoop computationLoop = computationLoop;
    private readonly TcpListener tcpListener = new(IPAddress.Parse("127.0.0.1"), 8080);
    private readonly Dictionary<Guid, TcpClient> _sockets = [];
    private readonly int _maxSockets = 10;
    // See RFC 6455, section 4.2.2
    private readonly string _magicOongaBoongaBoogaloo = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

    public void Start()
    {
        tcpListener.Start();

        while (true)
        {
            // Using the simple one client per thread model, limit active threads to some reasonable value
            // so we don't overwhelm system resources.
            TcpClient socket = tcpListener.AcceptTcpClient();
            if (_sockets.Count <= _maxSockets)
            {
                Guid socketId = Guid.NewGuid();
                _sockets.Add(socketId, socket);

                Thread socketThread = new(() => SocketReadLoop(socket, socketId));
                socketThread.Start();
            }
            else
            {
                socket.Close();
            }
        }
    }

    private void SocketReadLoop(TcpClient socket, Guid socketId)
    {
        byte[] buffer = new byte[1000];
        while (true)
        {
            try
            {
                if (!socket.Connected)
                {
                    socket.Close();
                    socket.Dispose();
                    _sockets.Remove(socketId);
                    return;
                }

                int bufferCount = socket.GetStream().Read(buffer);
                PrintByteArrayInAscii(buffer, bufferCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public static void PrintByteArrayInAscii(byte[] bytes, int byteCount)
    {
        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, byteCount));
    }
}