using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace AsteroidsServer.Src;

public class GameServer(ComputationLoop computationLoop)
{
    private readonly ComputationLoop computationLoop = computationLoop;
    private readonly HttpListener httpListener = new();
    private readonly Dictionary<Guid, WebSocket> _sockets = [];
    private readonly int _maxSockets = 10;

    public void Start()
    {
        httpListener.Prefixes.Add("http://127.0.0.1:8080/");
        httpListener.Start();

        while (true)
        {
            HttpListenerContext context = httpListener.GetContext();
            // Using the simple one client per thread model, limit active threads to some reasonable value
            // so we don't overwhelm system resources.
            if (_sockets.Count <= _maxSockets)
            {
                Thread socketThread = new(() => SocketReadLoop(context));
                socketThread.Start();
            }
            else
            {
                // Send service unavailable
                context.Response.StatusCode = 503;
                context.Response.OutputStream.Dispose();
            }
        }
    }

    private async void SocketReadLoop(HttpListenerContext context)
    {
        WebSocket socket;

        try
        {
            socket = (await context.AcceptWebSocketAsync(null)).WebSocket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Response.OutputStream.Dispose();
            return;
        }

        byte[] buffer = new byte[1000];
        Guid socketId = Guid.NewGuid();
        _sockets.Add(socketId, socket);

        while (true)
        {
            try
            {
                if (socket.State != WebSocketState.Open)
                {
                    socket.Dispose();
                    _sockets.Remove(socketId);
                    return;
                }

                WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                PrintByteArrayInAscii(buffer, result.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public static void PrintByteArrayInAscii(ArraySegment<byte> bytes, int byteCount)
    {
        Console.WriteLine(Encoding.ASCII.GetString([.. bytes], 0, byteCount));
    }
}