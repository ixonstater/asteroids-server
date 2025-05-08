using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace AsteroidsServer.Src;

public class GameServer(ComputationLoop computationLoop)
{
    private readonly ComputationLoop computationLoop = computationLoop;
    private readonly HttpListener httpListener = new();
    private readonly Dictionary<Guid, WebSocket> _sockets = [];
    public int SocketCount
    {
        get => _sockets.Count;
    }
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
            if (_sockets.Count < _maxSockets)
            {
                SocketReadLoop(context);
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
            // Connection fails for some reason
            Console.WriteLine(e);
            context.Response.OutputStream.Dispose();
            return;
        }

        // 1KB is good rite...?
        byte[] buffer = new byte[1000];
        Guid socketId = Guid.NewGuid();
        _sockets.Add(socketId, socket);

        // Do this after adding the socket to the Dictionary so that the computation loop doesn't immediately
        // close again.
        EnsureComputationLoop();

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

    private void EnsureComputationLoop()
    {
        if (!computationLoop.Running)
        {
            computationLoop.Start(() => { return SocketCount; });
        }
    }

    /// <summary>
    /// Just for testing, prints a byte array as ascii chars.
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="byteCount"></param>
    public static void PrintByteArrayInAscii(ArraySegment<byte> bytes, int byteCount)
    {
        Console.WriteLine(Encoding.ASCII.GetString([.. bytes], 0, byteCount));
    }
}