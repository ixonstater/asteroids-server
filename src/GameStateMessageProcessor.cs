using System.Net.WebSockets;
using AsteroidsServer.Src.Messages.Message;
using AsteroidsServer.Src.Messages.Ship;
using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src
{
    public class GameStateMessageProcessor
    {
        private Dictionary<string, WebSocket>? _sockets;
        public Dictionary<string, WebSocket> Sockets
        {
            set
            {
                // Only allow sockets to be set once
                _sockets ??= value;
            }
        }

        public async void BroadcastShipPositions(GameState gameState)
        {
            if (_sockets == null)
            {
                return;
            }

            byte[] msg = WriteShipPositionMessage(gameState.Ships);

            foreach (var socket in _sockets)
            {
                try
                {
                    await socket.Value.SendAsync(msg, WebSocketMessageType.Binary, true, CancellationToken.None);
                }
                catch (Exception)
                {
                    _sockets.Remove(socket.Key);
                    gameState.DeleteShip(socket.Key);
                    continue;
                }
            }
        }

        private static byte[] WriteShipPositionMessage(Dictionary<string, ShipEntity> ships)
        {
            return MessageUtils.Serialize(Ships.ToResponse(ships));
        }
    }
}