using System.Net.WebSockets;
using AsteroidsServer.Src.TrackedEntities;

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

        public async void BroadcastShipPositions(Dictionary<string, ShipEntity> ships)
        {
            if (_sockets == null)
            {
                return;
            }

            byte[] msg = WriteShipPositionMessage();

            foreach (WebSocket socket in _sockets.Values)
            {
                await socket.SendAsync(msg, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        private byte[] WriteShipPositionMessage() { }
    }
}