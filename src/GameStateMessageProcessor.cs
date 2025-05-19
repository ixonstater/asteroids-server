using System.Net.WebSockets;

namespace AsteroidsServer.Src
{
    public class GameStateMessageProcessor
    {
        private Dictionary<Guid, WebSocket>? _sockets;
        public Dictionary<Guid, WebSocket> Sockets
        {
            set
            {
                // Only allow sockets to be set once
                if (_sockets != null)
                {
                    _sockets = value;
                }
            }
        }
    }
}