using System.Net.Sockets;
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

        public void CleanUpSocketConnectionsAndGameState(GameState gameState)
        {
            if (_sockets == null)
            {
                return;
            }

            string[] deadConnections = new string[100];
            int deadConnectionIndex = 0;

            foreach (KeyValuePair<string, WebSocket> socket in _sockets)
            {
                if (!(socket.Value.State == WebSocketState.Connecting || socket.Value.State == WebSocketState.Open))
                {
                    socket.Value.Dispose();
                    deadConnections[deadConnectionIndex++] = socket.Key;
                }
            }

            foreach (string deadConnection in deadConnections)
            {
                if (deadConnection == null)
                {
                    break;
                }
                _sockets.Remove(deadConnection);
                DisposeConnectionGameState(gameState, deadConnection);
            }
        }

        private static void DisposeConnectionGameState(GameState gameState, string connectionId)
        {
            gameState.DeleteShip(connectionId);
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
                catch (Exception e)
                {
                    Console.WriteLine("Caught exception while broadcasting ship positions: " + e.ToString());
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